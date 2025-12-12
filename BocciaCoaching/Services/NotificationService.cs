using BocciaCoaching.Models.Entities;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Repositories.NotificationTypes;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationTypeRepository _repo;
        private readonly IUserRepository _userRepo;

        public NotificationService(INotificationTypeRepository repo, IUserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        private NotificationTypeDto MapType(NotificationType t)
        {
            return new NotificationTypeDto
            {
                NotificationTypeId = t.NotificationTypeId,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            };
        }

        private NotificationMessageDto MapMessage(NotificationMessage m)
        {
            return new NotificationMessageDto
            {
                NotificationMessageId = m.NotificationMessageId,
                Message = m.Message,
                Image = m.Image,
                SenderId = m.SenderId,
                // La entidad User tiene FirstName/LastName - validar que Sender no sea null
                SenderName = m.Sender?.FirstName != null ? (m.Sender.FirstName + (string.IsNullOrWhiteSpace(m.Sender.LastName) ? "" : " " + m.Sender.LastName)) : null,
                NotificationTypeId = m.NotificationTypeId,
                // Validar que Receiver no sea null
                ReceiverName = m.Receiver?.FirstName != null ? (m.Receiver.FirstName + (string.IsNullOrWhiteSpace(m.Receiver.LastName) ? "" : " " + m.Receiver.LastName)) : null,
                // Validar que NotificationType no sea null
                NotificationTypeName = m.NotificationType?.Name,
                ReceiverId = m.ReceiverId
            };
        }

        private async Task<NotificationMessageDto> MapMessageAsync(NotificationMessage m)
        {
            var dto = MapMessage(m);

            if (string.IsNullOrWhiteSpace(dto.SenderName))
            {
                var senderResponse = await _userRepo.GetByIdAsync(m.SenderId);
                if (senderResponse.Success && senderResponse.Data != null)
                {
                    var firstName = senderResponse.Data.Name ?? string.Empty;
                    var lastName = senderResponse.Data.LastName;
                    dto.SenderName = string.IsNullOrWhiteSpace(lastName) 
                        ? firstName.Trim() 
                        : (firstName + " " + lastName).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(dto.ReceiverName))
            {
                var receiverResponse = await _userRepo.GetByIdAsync(m.ReceiverId);
                if (receiverResponse.Success && receiverResponse.Data != null)
                {
                    var firstName = receiverResponse.Data.Name ?? string.Empty;
                    var lastName = receiverResponse.Data.LastName;
                    dto.ReceiverName = string.IsNullOrWhiteSpace(lastName) 
                        ? firstName.Trim() 
                        : (firstName + " " + lastName).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(dto.NotificationTypeName))
            {
                var nt = await _repo.GetByIdAsync(m.NotificationTypeId);
                dto.NotificationTypeName = nt?.Name;
            }

            return dto;
        }

        public async Task<ResponseContract<IEnumerable<NotificationTypeDto>>> GetAllTypes()
        {
            try
            {
                var types = await _repo.GetAllAsync();
                var dto = types.Select(MapType).ToList();
                return ResponseContract<IEnumerable<NotificationTypeDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<IEnumerable<NotificationTypeDto>>.Fail("Error obteniendo los tipos de notificación");
            }
        }

        public async Task<ResponseContract<NotificationTypeDto>> GetTypeById(int id)
        {
            try
            {
                var type = await _repo.GetByIdAsync(id);
                if (type == null) return ResponseContract<NotificationTypeDto>.Fail("Tipo no encontrado");

                return ResponseContract<NotificationTypeDto>.Ok(MapType(type));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<NotificationTypeDto>.Fail("Error obteniendo el tipo de notificación");
            }
        }

        public async Task<ResponseContract<bool>> CreateType(RequestCreateNotificationTypeDto? typeDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(typeDto?.Name))
                    return ResponseContract<bool>.Fail("El nombre del tipo es requerido");

                var type = new NotificationType
                {
                    Name = typeDto.Name,
                    Description = typeDto.Description,
                    Status = true,
                    CreatedAt = DateTime.Now
                };

                var result = await _repo.AddAsync(type);
                if (!result) return ResponseContract<bool>.Fail("No se pudo crear el tipo");
                return ResponseContract<bool>.Ok(true, "Tipo creado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<bool>.Fail("Error creando el tipo");
            }
        }

        public async Task<ResponseContract<bool>> UpdateType(RequestUpdateNotificationTypeDto? typeDto)
        {
            try
            {
                if (typeDto is null || typeDto.NotificationTypeId <= 0)
                    return ResponseContract<bool>.Fail("Tipo inválido");

                if (string.IsNullOrWhiteSpace(typeDto.Name))
                    return ResponseContract<bool>.Fail("El nombre del tipo es requerido");

                var type = new NotificationType
                {
                    NotificationTypeId = typeDto.NotificationTypeId,
                    Name = typeDto.Name,
                    Description = typeDto.Description,
                    Status = typeDto.Status
                };

                var result = await _repo.UpdateAsync(type);
                if (!result) return ResponseContract<bool>.Fail("No se pudo actualizar el tipo");
                return ResponseContract<bool>.Ok(true, "Tipo actualizado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<bool>.Fail("Error actualizando el tipo");
            }
        }

        public async Task<ResponseContract<NotificationMessageDto>> GetMessageById(int id)
        {
            try
            {
                var message = await _repo.GetMessageByIdAsync(id);
                if (message == null) return ResponseContract<NotificationMessageDto>.Fail("Mensaje no encontrado");
                var dto = await MapMessageAsync(message);
                return ResponseContract<NotificationMessageDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<NotificationMessageDto>.Fail("Error obteniendo el mensaje");
            }
        }

        private async Task<(bool exists, string? name)> GetUserInfo(int userId)
        {
            var userResponse = await _userRepo.GetByIdAsync(userId);
            
            if (!userResponse.Success || userResponse.Data == null) 
                return (false, null);
            
            var first = userResponse.Data.Name ?? string.Empty;
            var last = userResponse.Data.LastName;
            var fullName = string.IsNullOrWhiteSpace(last) ? first.Trim() : (first + " " + last).Trim();
            return (true, string.IsNullOrWhiteSpace(fullName) ? null : fullName);
        }

        public async Task<ResponseContract<bool>> CreateMessage(RequestCreateNotificationMessageDto? messageDto)
        {
            try
            {
                if (messageDto is null)
                    return ResponseContract<bool>.Fail("Mensaje inválido");

                // Validar que exista el tipo de notificación
                var type = await _repo.GetByIdAsync(messageDto.NotificationTypeId);
                if (type == null)
                    return ResponseContract<bool>.Fail("Tipo de notificación inválido");

                // Validar sender y receiver
                var (senderExists, _) = await GetUserInfo(messageDto.SenderId);
                if (!senderExists) return ResponseContract<bool>.Fail("Remitente inválido");
                var (receiverExists, _) = await GetUserInfo(messageDto.ReceiverId);
                if (!receiverExists) return ResponseContract<bool>.Fail("Destinatario inválido");

                var message = new NotificationMessage
                {
                    Message = messageDto.Message,
                    Image = messageDto.Image,
                    SenderId = messageDto.SenderId,
                    ReceiverId = messageDto.ReceiverId,
                    NotificationTypeId = messageDto.NotificationTypeId,
                    Status = messageDto.Status
                };

                var result = await _repo.AddMessageAsync(message);
                if (!result) return ResponseContract<bool>.Fail("No se pudo crear el mensaje");
                return ResponseContract<bool>.Ok(true, "Mensaje creado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<bool>.Fail("Error creando el mensaje");
            }
        }

        public async Task<ResponseContract<bool>> UpdateMessage(RequestUpdateNotificationMessageDto? messageDto)
        {
            try
            {
                if (messageDto is null || messageDto.NotificationMessageId <= 0)
                    return ResponseContract<bool>.Fail("Mensaje inválido");

                // Validar que exista el tipo de notificación
                var type = await _repo.GetByIdAsync(messageDto.NotificationTypeId);
                if (type == null)
                    return ResponseContract<bool>.Fail("Tipo de notificación inválido");

                var (senderExists, _) = await GetUserInfo(messageDto.SenderId);
                if (!senderExists) return ResponseContract<bool>.Fail("Remitente inválido");
                var (receiverExists, _) = await GetUserInfo(messageDto.ReceiverId);
                if (!receiverExists) return ResponseContract<bool>.Fail("Destinatario inválido");

                var message = new NotificationMessage
                {
                    NotificationMessageId = messageDto.NotificationMessageId,
                    Message = messageDto.Message,
                    Image = messageDto.Image,
                    SenderId = messageDto.SenderId,
                    ReceiverId = messageDto.ReceiverId,
                    NotificationTypeId = messageDto.NotificationTypeId,
                    Status = messageDto.Status
                };

                var result = await _repo.UpdateMessageAsync(message);
                if (!result) return ResponseContract<bool>.Fail("No se pudo actualizar el mensaje");
                return ResponseContract<bool>.Ok(true, "Mensaje actualizado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<bool>.Fail("Error actualizando el mensaje");
            }
        }

        public async Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByCoach(int coachId, int? page = null, int? pageSize = null, bool? status = null)
        {
            try
            {
                var messages = await _repo.GetMessagesByCoachAsync(coachId);
                
                if (messages == null || !messages.Any())
                {
                    return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(
                        new List<NotificationMessageDto>(), 
                        "No se encontraron mensajes para este coach"
                    );
                }

                var query = messages.AsQueryable();
                if (status.HasValue) query = query.Where(m => m.Status == status.Value);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var dto = new List<NotificationMessageDto>();
                foreach (var message in query)
                {
                    try
                    {
                        var mappedMessage = await MapMessageAsync(message);
                        dto.Add(mappedMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error mapeando mensaje {message.NotificationMessageId}: {ex.Message}");
                        // Continuar con el siguiente mensaje
                    }
                }

                return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetMessagesByCoach: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Fail("Error obteniendo mensajes por coach");
            }
        }

        public async Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByAthlete(int athleteId, int? page = null, int? pageSize = null, bool? status = null)
        {
            try
            {
                var messages = await _repo.GetMessagesByAthleteAsync(athleteId);
                
                if (messages == null || !messages.Any())
                {
                    return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(
                        new List<NotificationMessageDto>(), 
                        "No se encontraron mensajes para este atleta"
                    );
                }

                var query = messages.AsQueryable();
                //  if (status.HasValue) query = query.Where(m => m.Status == status.Value);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var dto = new List<NotificationMessageDto>();
                foreach (var message in query)
                {
                    try
                    {
                        var mappedMessage = await MapMessageAsync(message);
                        dto.Add(mappedMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error mapeando mensaje {message.NotificationMessageId}: {ex.Message}");
                        // Continuar con el siguiente mensaje
                    }
                }

                return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetMessagesByAthlete: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Fail("Error obteniendo mensajes por atleta");
            }
        }
    }
}
