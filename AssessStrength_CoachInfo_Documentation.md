# Modificación del Endpoint GetActiveEvaluation - Información del Entrenador

## Resumen
Se modificó el endpoint `/api/AssessStrength/GetActiveEvaluation/{teamId}` para incluir información del entrenador que creó la evaluación de fuerza activa.

## Cambios Realizados

### 1. Modificación del DTO `ActiveEvaluationDto`
**Archivo:** `BocciaCoaching/Models/DTO/AssessStrength/ActiveEvaluationDto.cs`

**Campos agregados:**
```csharp
public int CreatedByCoachId { get; set; }
public string? CreatedByCoachName { get; set; }
public string? CreatedByCoachEmail { get; set; }
```

### 2. Modificación del Repositorio `AssessStrengthRepository`
**Archivo:** `BocciaCoaching/Repositories/AssesstStrength/AssessStrengthRepository.cs`

**Método modificado:** `GetActiveEvaluationWithDetailsAsync`

**Cambios principales:**
- Se agregó el `ThenInclude` para cargar la información del entrenador del equipo
- Se modificó la construcción del DTO para incluir la información del entrenador

**Código agregado:**
```csharp
// Incluir información del entrenador del equipo
.Include(a => a.Team)
    .ThenInclude(t => t != null ? t.Coach : null)

// En la construcción del DTO
CreatedByCoachId = activeAssessment.Team?.CoachId ?? 0,
CreatedByCoachName = activeAssessment.Team?.Coach != null 
    ? $"{activeAssessment.Team.Coach.FirstName} {activeAssessment.Team.Coach.LastName}" 
    : "Entrenador desconocido",
CreatedByCoachEmail = activeAssessment.Team?.Coach?.Email,
```

## Lógica de Negocio

### Relación de Datos
1. **AssessStrength** → pertenece a un **Team**
2. **Team** → tiene un **CoachId** que referencia al **User** (entrenador)
3. Por lo tanto, el entrenador que crea la evaluación es el entrenador asignado al equipo

### Campos de Respuesta
- **CreatedByCoachId**: ID del entrenador que creó la evaluación (obtenido del equipo)
- **CreatedByCoachName**: Nombre completo del entrenador (FirstName + LastName)
- **CreatedByCoachEmail**: Email del entrenador

### Validaciones de Seguridad
- Se agregaron validaciones de nulos para evitar errores de compilación
- Si no se encuentra el entrenador, se muestra "Entrenador desconocido"
- Se usa el operador `??` para asignar 0 si no hay CoachId

## Ejemplo de Respuesta

```json
{
  "isSuccess": true,
  "message": "Evaluación activa encontrada",
  "data": {
    "assessStrengthId": 123,
    "evaluationDate": "2024-12-20T10:00:00Z",
    "description": "Evaluación de fuerza mensual",
    "state": "A",
    "teamId": 5,
    "teamName": "Equipo Élite Boccia",
    "createdByCoachId": 15,
    "createdByCoachName": "Juan Carlos Pérez",
    "createdByCoachEmail": "juan.perez@email.com",
    "createdAt": "2024-12-20T09:30:00Z",
    "updatedAt": null,
    "athletes": [...],
    "throws": [...]
  }
}
```

## Impacto en el Sistema
- ✅ No afecta la funcionalidad existente
- ✅ Agrega información útil para identificar quién creó la evaluación
- ✅ Mantiene compatibilidad con clientes existentes (campos nuevos pueden ser ignorados)
- ✅ No requiere cambios en el controlador ni en el servicio

## Testing
El proyecto compila correctamente sin errores. Los warnings existentes son menores y no afectan la funcionalidad nueva.
