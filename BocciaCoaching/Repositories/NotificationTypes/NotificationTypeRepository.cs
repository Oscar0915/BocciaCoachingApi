using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.NotificationTypes
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(NotificationType? notificationType)
        {
            try
            {
                if (notificationType is null) return false;

                notificationType.CreatedAt = DateTime.Now;

                await _context.NotificationType.AddAsync(notificationType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en NotificationTypeRepository.AddAsync: {ex.Message}");
                return false;
            }
        }

        // Obtener todos los tipos de notificación
        public async Task<IEnumerable<NotificationType>> GetAllAsync()
        {
            return await _context.NotificationType
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener por id
        public async Task<NotificationType?> GetByIdAsync(int id)
        {
            return await _context.NotificationType
                .AsNoTracking()
                .FirstOrDefaultAsync(nt => nt.NotificationTypeId == id);
        }

        // Actualizar NotificationType
        public async Task<bool> UpdateAsync(NotificationType? notificationType)
        {
            try
            {
                if (notificationType is null) return false;

                var existing = await _context.NotificationType.FirstOrDefaultAsync(nt => nt.NotificationTypeId == notificationType.NotificationTypeId);
                if (existing == null) return false;

                // Actualizar campos permitidos
                existing.Name = notificationType.Name;
                existing.Description = notificationType.Description;
                existing.Status = notificationType.Status;
                existing.UpdatedAt = DateTime.Now;

                _context.NotificationType.Update(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en NotificationTypeRepository.UpdateAsync: {ex.Message}");
                return false;
            }
        }

        // Agregar mensaje de notificación
        public async Task<bool> AddMessageAsync(NotificationMessage? message)
        {
            try
            {
                if (message is null) return false;

                await _context.NotificationMessage.AddAsync(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en NotificationTypeRepository.AddMessageAsync: {ex.Message}");
                return false;
            }
        }

        // Actualizar mensaje
        public async Task<bool> UpdateMessageAsync(NotificationMessage? message)
        {
            try
            {
                if (message is null) return false;

                var existing = await _context.NotificationMessage.FirstOrDefaultAsync(m => m.NotificationMessageId == message.NotificationMessageId);
                if (existing == null) return false;

                existing.Message = message.Message;
                existing.Status = message.Status;
                existing.Image = message.Image;
                existing.SenderId = message.SenderId;
                existing.ReceiverId = message.ReceiverId;
                existing.NotificationTypeId = message.NotificationTypeId;
                existing.ReferenceId = message.ReferenceId;

                _context.NotificationMessage.Update(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en NotificationTypeRepository.UpdateMessageAsync: {ex.Message}");
                return false;
            }
        }

        // Obtener mensaje por id (incluye relaciones)
        public async Task<NotificationMessage?> GetMessageByIdAsync(int id)
        {
            return await _context.NotificationMessage
                .Include(nm => nm.Sender)
                .Include(nm => nm.Receiver)
                .Include(nm => nm.NotificationType)
                .AsNoTracking()
                .FirstOrDefaultAsync(nm => nm.NotificationMessageId == id);
        }

        public async Task<IEnumerable<NotificationMessage>> GetMessagesByCoachAsync(int coachId)
        {
            return await _context.NotificationMessage
                .Where(m => m.ReceiverId == coachId)
                .Include(m => m.NotificationType)
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<NotificationMessage>> GetMessagesByAthleteAsync(int athleteId)
        {
            return await _context.NotificationMessage
                .Where(m => m.ReceiverId == athleteId)
                .Include(m => m.NotificationType)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
