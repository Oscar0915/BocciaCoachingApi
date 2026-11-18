# API v1 — estándar y guía de migración

## Convenciones

- **Rutas REST** bajo `/api/v1`, en minúsculas y en plural (`/teams`, `/wellness`). Los recursos hijos van anidados: `/teams/{teamId}/members`.
- **Verbos HTTP**: `GET` lee, `POST` crea, `PUT` actualiza. Nada de `POST` para consultar.
- **El identificador va en la ruta**, no en el cuerpo. Si el cuerpo también trae el ID, manda el de la ruta.
- **Mismo cuerpo de siempre**: `{ "success": bool, "message": string, "data": T }`.
- **Códigos HTTP reales**:

| Código | Cuándo |
|---|---|
| 200 | OK |
| 201 | Creado (incluye cabecera `Location`) |
| 400 | Datos inválidos. En validación, `data` = `{ campo: [errores] }` |
| 404 | No existe el recurso |
| 409 | Conflicto (ya existe, duplicado) |
| 500 | Error inesperado. Mensaje genérico + cabecera `X-Trace-Id` para buscar en logs |

- **Rutas antiguas** (`/api/Team/...`, `/api/Wellness/...`) siguen funcionando igual: siempre HTTP 200 con `success` en el cuerpo. Aparecen como *deprecated* en Swagger.
- `GET /health` para el health check del hosting.

## Equivalencias — Teams

| Antes | Ahora |
|---|---|
| `POST /api/Team/AddNewTeam` | `POST /api/v1/teams` → 201 |
| — | `GET /api/v1/teams/{teamId}` |
| `GET /api/Team/GetTeamsForUser/{coachId}` | `GET /api/v1/users/{userId}/teams` |
| `POST /api/Team/GetUsersForTeam` `{teamId, rolId}` | `GET /api/v1/teams/{teamId}/members?roleId=` (sin `roleId` = todos) |
| `POST /api/Team/AddNewTeamMember` `{teamId, userId}` | `POST /api/v1/teams/{teamId}/members` `{userId}` → 201 / 409 |
| `POST /api/Team/UpdateTeam` (devolvía `bool`) | `PUT /api/v1/teams/{teamId}` (devuelve el contrato estándar) |
| `POST /api/Team/GetRecentStatistics` `{coachId, teamId}` | `GET /api/v1/teams/{teamId}/strength-statistics/recent?coachId=` |

## Equivalencias — Wellness

| Antes | Ahora |
|---|---|
| `POST /api/Wellness/AddDailyWellness` | `POST /api/v1/wellness` → 201 / 409 si ya existe ese día |
| `PUT /api/Wellness/UpdateDailyWellness` | `PUT /api/v1/wellness/{dailyWellnessId}` |
| `GET /api/Wellness/GetWellnessById/{id}` | `GET /api/v1/wellness/{dailyWellnessId}` |
| `GET /api/Wellness/GetTodayWellness/{athleteId}` | `GET /api/v1/athletes/{athleteId}/wellness/today` (404 si no lo ha registrado) |
| `GET /api/Wellness/GetWellnessByDate/{athleteId}/{date}` | `GET /api/v1/athletes/{athleteId}/wellness/{yyyy-MM-dd}` |
| `GET /api/Wellness/GetAthleteHistory/{athleteId}` | `GET /api/v1/athletes/{athleteId}/wellness` |
| `GET /api/Wellness/GetTeamWellnessByDate/{teamId}/{date}` | `GET /api/v1/teams/{teamId}/wellness?date=yyyy-MM-dd` (sin fecha = hoy) |

> En el frontend: con HTTP 4xx/5xx, Angular `HttpClient` entra por `error`. El cuerpo sigue en `err.error` (`err.error.message`).

## Cómo migrar otro módulo (receta)

1. **Repositorio**: devuelve datos (DTO, entidad, `bool`), no `ResponseContract`. Sin `try/catch` que trague excepciones. Lecturas con `AsNoTracking()` + `Select(...)` hacia el DTO; nada de `Include` + mapear en memoria. Fechas por rango (`>= inicio && < inicio+1 día`), no `.Date ==`. Recibir `CancellationToken`.
2. **Servicio**: reglas de negocio y mensajes. Usa `ResponseContract<T>.Ok / Created / Invalid / NotFound / Conflict`. Errores inesperados: no capturar; los maneja `GlobalExceptionHandler`.
3. **Controlador v1** en `Controllers/V1`, heredando de `ApiControllerBase`. Responde con `FromResult(...)` o `CreatedFromResult(...)`.
4. **Controlador antiguo**: añadir `[LegacyApi]` y `[Obsolete("Usar /api/v1/...")]`, llamar al servicio nuevo y devolver `Ok(...)`.
