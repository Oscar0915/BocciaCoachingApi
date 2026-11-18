# Cuestionario diario de bienestar (McLean 2010)

## Reglas

- **5 ítems de 1 (pésimo) a 5 (excelente)**: `fatigue`, `sleep`, `musclePain`, `stress` y `mood`. Total de 5 a 25.
- **Semáforo** por puntaje total:

| Semáforo | Total | Estado |
|---|---|---|
| Verde (`Green`) | 20–25 | Óptimo |
| Amarillo (`Yellow`) | 15–19 | Precaución |
| Rojo (`Red`) | 5–14 | Peligro |

- **Veto individual**: si el total da Verde pero algún ítem es ≤ 2, el semáforo baja a Amarillo (`vetoApplied: true`).
- **Alertas por ítem**: se generan para cada ítem ≤ 2, en cualquier nivel.
- **Un registro por atleta por día.** "Hoy" es el día local del atleta:
  - Zona horaria: cabecera `X-Time-Zone` (IANA, p. ej. `America/Bogota`).
  - Si no viene la cabecera, se usa `Wellness:DefaultTimeZone` (por defecto `America/Bogota`). El servidor corre en UTC.
- **Permisos**:
  - **Atleta**: registra y corrige solo el suyo, y solo el de hoy.
  - **Entrenador**: lee y registra el de los atletas de sus equipos, también de días pasados (útil si el atleta necesita asistencia).
  - **Admin**: todo.
- **Registros anteriores** (escala 1–7, 4 ítems): se conservan con `scaleVersion: 1`, sin semáforo ni feedback, y no cuentan en los promedios.
- **Mensajes**: el backend los calcula al responder y no se guardan, así que se pueden ajustar los textos sin migrar datos. Los textos para Verde y las alertas de Fatiga y Ánimo los propuso Claude; los demás son los de la especificación.

## Endpoints (`/api/v1`, con `Authorization: Bearer`)

| Método | Ruta | Uso |
|---|---|---|
| GET | `/wellness/questionnaire` | Preguntas y etiquetas para pintar el formulario |
| GET | `/wellness/me/today` | **Al ingresar a la app.** `data.mustComplete = true` significa que hay que mostrar el cuestionario |
| POST | `/wellness/me` | El atleta envía el de hoy → `201` con semáforo y feedback; `409` si ya lo hizo |
| PUT | `/wellness/{id}` | Corregir (el atleta, solo el de hoy) |
| GET | `/wellness/{id}` | Un registro |
| POST | `/wellness` | Registrar para un atleta (`athleteId`; entrenador o admin, opcional `assessmentDate`) |
| GET | `/athletes/{athleteId}/wellness` | Historial, con días en verde, amarillo y rojo |
| GET | `/athletes/{athleteId}/wellness/today` | El de hoy de un atleta (404 si no lo hizo) |
| GET | `/teams/{teamId}/wellness/summary?date=` | **Entrenador**: conteo por semáforo, rojos primero y atletas pendientes |
| GET | `/teams/{teamId}/wellness?date=` | Registros del equipo en una fecha |

Las rutas antiguas `/api/Wellness/...` siguen funcionando con las mismas reglas (siempre HTTP 200).

## Flujo en la app

1. Tras el login, o al abrir la app, llama a `GET /api/v1/wellness/me/today` con la cabecera `X-Time-Zone` (`Intl.DateTimeFormat().resolvedOptions().timeZone`).
2. Si `mustComplete` es true, muestra el cuestionario de forma obligatoria. Pinta las preguntas desde `GET /wellness/questionnaire`.
3. Envía `POST /api/v1/wellness/me`:
   ```json
   { "fatigue": 4, "sleep": 3, "musclePain": 2, "stress": 4, "mood": 5, "observations": "opcional", "teamId": null }
   ```
4. Muestra `data.feedback`:
   - Color según `level`, más `title` y `diagnosis`.
   - Listas `trainingActions` y `lifestyleActions`.
   - Cada elemento de `itemAlerts` como alerta destacada.
5. Si `mustComplete` es false y viene `assessment`, muestra el resultado de hoy.

Respuesta de `POST /wellness/me` (`data`):

```json
{
  "dailyWellnessId": "…", "assessmentDate": "2026-11-18T00:00:00", "scaleVersion": 2,
  "fatigue": 5, "sleep": 5, "musclePain": 1, "stress": 5, "mood": 5,
  "totalScore": 21, "averageScore": 4.2, "readinessLevel": "Yellow", "vetoApplied": true,
  "feedback": {
    "level": "Yellow", "status": "Precaución", "title": "Modificación Inteligente",
    "diagnosis": "Tu puntaje general es bueno, pero reportaste un indicador crítico. Ajusta tu sesión de hoy.",
    "trainingActions": ["Reduce la intensidad de tu sesión de hoy un 20%.", "…"],
    "lifestyleActions": ["Intenta dormir 30 minutos extra esta noche.", "…"],
    "itemAlerts": [{ "item": "MusclePain", "score": 1, "message": "Atención: Reportaste dolor muscular agudo. …" }]
  }
}
```

## Despliegue

Antes de desplegar, aplica la migración `McLeanWellnessQuestionnaire` (`dotnet ef database update`) o el script `mclean_wellness.sql`. Agrega 4 columnas a `DailyWellness`; los registros existentes quedan como `scaleVersion = 1`.
