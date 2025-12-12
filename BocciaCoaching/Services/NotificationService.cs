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
                CoachId = m.CoachId,
                // La entidad User tiene FirstName/LastName
                CoachName = m.Coach.FirstName != null ? (m.Coach.FirstName + (string.IsNullOrWhiteSpace(m.Coach.LastName) ? "" : " " + m.Coach.LastName)) : null,
                AthleteId = m.AthleteId,
                AthleteName = m.Athlete.FirstName != null ? (m.Athlete.FirstName + (string.IsNullOrWhiteSpace(m.Athlete.LastName) ? "" : " " + m.Athlete.LastName)) : null,
                NotificationTypeId = m.NotificationTypeId,
                NotificationTypeName = m.NotificationType.Name,
                Status = m.Status
            };
        }

        private async Task<NotificationMessageDto> MapMessageAsync(NotificationMessage m)
        {
            var dto = MapMessage(m);

            if (string.IsNullOrWhiteSpace(dto.CoachName))
            {
                var coach = await _userRepo.GetByIdAsync(m.CoachId);
                dto.CoachName = coach?.Name;
            }

            if (string.IsNullOrWhiteSpace(dto.AthleteName))
            {
                var athlete = await _userRepo.GetByIdAsync(m.AthleteId);
                dto.AthleteName = athlete?.Name;
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
            // IUserRepository.GetByIdAsync devuelve InfoBasicUserDto (Name, LastName)
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return (false, null);
            var first = user.Name ?? string.Empty;
            var last = user.LastName; // LastName es no-nullable en InfoBasicUserDto
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

                // Validar coach y athlete
                var (coachExists, _) = await GetUserInfo(messageDto.CoachId);
                if (!coachExists) return ResponseContract<bool>.Fail("Coach inválido");
                var (athleteExists, _) = await GetUserInfo(messageDto.AthleteId);
                if (!athleteExists) return ResponseContract<bool>.Fail("Atleta inválido");

                var message = new NotificationMessage
                {
                    Message = messageDto.Message,
                    Image = messageDto.Image,
                    CoachId = messageDto.CoachId,
                    AthleteId = messageDto.AthleteId,
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

                var (coachExists, _) = await GetUserInfo(messageDto.CoachId);
                if (!coachExists) return ResponseContract<bool>.Fail("Coach inválido");
                var (athleteExists, _) = await GetUserInfo(messageDto.AthleteId);
                if (!athleteExists) return ResponseContract<bool>.Fail("Atleta inválido");

                var message = new NotificationMessage
                {
                    NotificationMessageId = messageDto.NotificationMessageId,
                    Message = messageDto.Message,
                    Image = messageDto.Image,
                    CoachId = messageDto.CoachId,
                    AthleteId = messageDto.AthleteId,
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
                var query = messages.AsQueryable();
                if (status.HasValue) query = query.Where(m => m.Status == status.Value);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var tasks = query.Select(m => MapMessageAsync(m)).ToList();
                var dtoArray = await Task.WhenAll(tasks);
                var dto = dtoArray.ToList();
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Fail("Error obteniendo mensajes por coach");
            }
        }

        public async Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByAthlete(int athleteId, int? page = null, int? pageSize = null, bool? status = null)
        {
            try
            {
                var messages = await _repo.GetMessagesByAthleteAsync(athleteId);
                var query = messages.AsQueryable();
                if (status.HasValue) query = query.Where(m => m.Status == status.Value);

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var tasks = query.Select(m => MapMessageAsync(m)).ToList();
                var dtoArray = await Task.WhenAll(tasks);
                var dto = dtoArray.ToList();
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResponseContract<IEnumerable<NotificationMessageDto>>.Fail("Error obteniendo mensajes por atleta");
            }
        }
    }
}
