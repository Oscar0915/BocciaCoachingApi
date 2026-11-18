# Autenticación y protección de la API

## Resumen

- **Todas** las APIs exigen `Authorization: Bearer <accessToken>`, salvo las públicas listadas abajo. Un endpoint nuevo queda protegido por defecto.
- **Access token**: JWT firmado (HS256), dura **15 min**. Lleva `sub` (UserId), `email` y `role` (`Admin`, `Coach`, `Athlete`).
- **Refresh token**: valor aleatorio, dura **14 días**. En la BD solo se guarda su hash SHA-256. **Cada uso lo rota**: el anterior deja de servir. Si alguien reutiliza uno ya rotado (señal de robo), se cierra toda esa sesión.
- **Rate limiting**: login, refresh, códigos, registro y restablecimiento permiten 20 peticiones por minuto por IP y por endpoint (luego `429`).
- Errores con el contrato de siempre `{ success:false, message, data:null }`:
  - `401` = sin token, token inválido o expirado. Si expiró, la cabecera `WWW-Authenticate` incluye `expired`.
  - `403` = autenticado pero sin permiso.

## Configuración (obligatoria en producción)

| Variable | Valor |
|---|---|
| `Jwt__SigningKey` | Secreto aleatorio de 32+ bytes. Generar con `openssl rand -base64 48`. **Sin ella la app no arranca fuera de Development.** |
| `Jwt__AccessTokenMinutes` | Opcional (por defecto 15) |
| `Jwt__RefreshTokenDays` | Opcional (por defecto 14) |

En Development, si no hay clave, se genera una aleatoria en cada arranque.

Base de datos: aplicar la migración `AddRefreshTokens` (crea la tabla `RefreshToken`). Se puede usar `dotnet ef database update` o el script `add_refresh_tokens.sql`.

## Endpoints públicos (sin token)

- `POST /api/v1/auth/login`, `/google`, `/refresh`, `/logout`
- `POST /api/v1/auth/password-reset/request`, `/verify`, `/confirm`
- `POST /api/User/login`, `/login/google`, `/verify-email/google`, `/register/google`
- `POST /api/User/AddInfoUser`, `/ValidateEmail`, `/ResetPassword`
- `POST /api/Email/SendCodeVerify`, `/ValidateCode`
- `GET /api/Rol/GetAll`, `/GetById/{id}`
- `GET /api/Subscription/types`, `/types/{id}`
- `POST /api/Subscription/webhooks/stripe` (protegido por la firma de Stripe)
- `GET /health`

## Solo Admin

- **Rol**: Create, Update, Delete.
- **Notification**: CreateType, UpdateType.
- **Subscription**: POST, PUT y DELETE de `types`, `admin/all`, `admin/statistics`.
- **Diagnóstico de correo**: `Email/TestSmtpConnectivity`, `Email/SendTestEmail` y todo `EmailTest`.
- **Endpoints de depuración**: `DebugEvaluations` y `DebugTeamEvaluations`.

## Flujo en el frontend

### 1. Login

Las rutas antiguas también funcionan y devuelven lo mismo:

```http
POST /api/v1/auth/login        (o POST /api/User/login)
{ "email": "...", "password": "..." }
```

```json
{ "success": true, "data": {
    "userId": "...", "firstName": "...", "rolId": "...",
    "tokens": {
      "tokenType": "Bearer",
      "accessToken": "eyJ...", "accessTokenExpiresAt": "2026-11-17T06:00:00Z",
      "refreshToken": "q1w2...", "refreshTokenExpiresAt": "2026-12-01T05:45:00Z"
} } }
```

Guarda el `accessToken` en memoria y el `refreshToken` en `localStorage`. Al iniciar la app, si hay refresh token, llama a `/refresh` para recuperar la sesión.

### 2. Cada petición

Añade `Authorization: Bearer <accessToken>` con un interceptor.

### 3. Al recibir 401

1. Llama **una sola vez** a `/refresh`. Si llegan varias peticiones con 401 a la vez, que esperen a ese único refresh; no lances uno por cada petición.
2. Si responde 200, guarda **los dos tokens nuevos** y reintenta la petición original.
3. Si `/refresh` también devuelve 401, la sesión terminó: manda al usuario al login.

```http
POST /api/v1/auth/refresh
{ "refreshToken": "q1w2..." }   →  data = { accessToken, refreshToken, ... }
```

Ejemplo de interceptor Angular:

```ts
intercept(req: HttpRequest<unknown>, next: HttpHandler) {
  const authReq = this.auth.accessToken
    ? req.clone({ setHeaders: { Authorization: `Bearer ${this.auth.accessToken}` } })
    : req;
  return next.handle(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status !== 401 || req.url.includes('/auth/')) return throwError(() => err);
      return this.auth.refreshOnce().pipe(          // comparte un único refresh en curso (shareReplay)
        switchMap(t => next.handle(req.clone({ setHeaders: { Authorization: `Bearer ${t.accessToken}` } }))),
        catchError(e => { this.auth.logoutLocal(); this.router.navigate(['/login']); return throwError(() => e); })
      );
    })
  );
}
```

### 4. Logout

```http
POST /api/v1/auth/logout          { "refreshToken": "..." }
POST /api/v1/auth/logout-all      (con access token: cierra todos los dispositivos)
```

Después borra los tokens locales. El access token sigue siendo válido hasta 15 minutos: es el comportamiento normal de JWT.

## Cambios que afectan al frontend

| Endpoint | Antes | Ahora |
|---|---|---|
| Todas las APIs | Abiertas | Requieren `Authorization: Bearer` |
| `POST /api/Email/ValidateCode` | `{ message, stateCode }` | Además devuelve `resetToken` si el código es válido. Máximo 5 intentos por código. |
| `POST /api/User/ResetPassword` | `{ email, newPassword }` | **Exige** `{ email, newPassword, resetToken }`. El token es de un solo uso y dura 10 min. Cierra todas las sesiones. |
| `PUT /api/User/UpdatePassword` | Aceptaba solo email, sin contraseña actual | El usuario sale del token y se exige `currentPassword`. Cierra todas las sesiones: vuelve a hacer login, o usa `POST /api/v1/auth/change-password`, que devuelve tokens nuevos. |
| `PUT /api/User/UpdateUserInfo` | Cualquiera editaba a cualquiera | Solo el propio usuario (o un Admin). Si no, 403. |
| `POST /api/User/AddInfoUser` | Cualquier rol | Solo `Coach` o `Athlete` |
| `POST /api/User/login` | Mensajes "Usuario no encontrado" / "Contraseña incorrecta" | Mensaje único "Credenciales inválidas", para no revelar qué emails existen |

### Flujo de "olvidé mi contraseña"

1. `POST /api/Email/SendCodeVerify` con `{ toEmail }` (o `POST /api/v1/auth/password-reset/request` con `{ email }`).
2. `POST /api/Email/ValidateCode` con `{ toEmail, code }`, que devuelve `resetToken` (o `/api/v1/auth/password-reset/verify`).
3. `POST /api/User/ResetPassword` con `{ email, newPassword, resetToken }` (o `/api/v1/auth/password-reset/confirm`).

## Pendiente (siguiente fase)

Autenticar no es lo mismo que autorizar cada recurso. Hoy cualquier usuario con sesión puede leer o modificar datos de otro si conoce sus IDs (equipos, evaluaciones, wellness, suscripciones…), porque esos endpoints reciben `coachId`, `athleteId` o `userId` del cliente. El siguiente paso es tomar el usuario de `User.GetUserId()` y validar en cada servicio que el recurso le pertenece.
