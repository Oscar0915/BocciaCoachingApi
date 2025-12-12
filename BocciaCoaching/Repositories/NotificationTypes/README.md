# NotificationType Repository

Repositorio para administrar `NotificationType`.

Interfaz: `INotificationTypeRepository`.
Implementación: `NotificationTypeRepository`.

Método principal:
- `Task<bool> AddAsync(NotificationType notificationType)` - guarda un nuevo tipo de notificación y establece `CreatedAt`.

Registro DI (ya añadido en `Program.cs`):

builder.Services.AddScoped<BocciaCoaching.Repositories.NotificationTypes.INotificationTypeRepository, BocciaCoaching.Repositories.NotificationTypes.NotificationTypeRepository>();

Uso rápido (ejemplo en un servicio constructor):

```csharp
private readonly INotificationTypeRepository _notificationTypeRepository;

public MyService(INotificationTypeRepository notificationTypeRepository)
{
    _notificationTypeRepository = notificationTypeRepository;
}

public async Task CreateType()
{
    var type = new NotificationType { Name = "Mi tipo", Description = "Desc" };
    await _notificationTypeRepository.AddAsync(type);
}
```

