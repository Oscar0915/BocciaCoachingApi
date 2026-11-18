using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);

        Task<RefreshToken?> FindRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Rota de forma atómica: revoca <paramref name="current"/> solo si sigue activo e inserta
        /// <paramref name="replacement"/>. Devuelve false si otra petición lo rotó primero.
        /// </summary>
        Task<bool> RotateRefreshTokenAsync(RefreshToken current, RefreshToken replacement, CancellationToken cancellationToken = default);

        Task RevokeFamilyAsync(Guid familyId, string reason, CancellationToken cancellationToken = default);

        Task RevokeAllForUserAsync(Guid userId, string reason, CancellationToken cancellationToken = default);

        /// <summary>ES: Limpieza: borra los tokens del usuario vencidos hace más de <paramref name="olderThan"/>.</summary>
        Task DeleteExpiredAsync(Guid userId, TimeSpan olderThan, CancellationToken cancellationToken = default);

        /// <summary>ES: Datos mínimos del usuario para emitir un token (null si no existe).</summary>
        Task<TokenSubject?> GetTokenSubjectAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default);
    }

    /// <summary>ES: Usuario y roles con los que se firma un access token.</summary>
    public sealed record TokenSubject(Guid UserId, string? Email, IReadOnlyList<Guid> RoleIds);

    public class AuthRepository(ApplicationDbContext context) : IAuthRepository
    {
        public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
        {
            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task<RefreshToken?> FindRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
            => context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        public async Task<bool> RotateRefreshTokenAsync(RefreshToken current, RefreshToken replacement, CancellationToken cancellationToken = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var now = DateTime.UtcNow;
            // UPDATE condicional: solo una de dos peticiones concurrentes con el mismo token gana.
            var revoked = await context.RefreshTokens
                .Where(t => t.RefreshTokenId == current.RefreshTokenId && t.RevokedAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.RevokedAt, now)
                    .SetProperty(t => t.RevokedReason, "rotated")
                    .SetProperty(t => t.ReplacedByTokenId, replacement.RefreshTokenId), cancellationToken);

            if (revoked == 0)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }

            context.RefreshTokens.Add(replacement);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }

        public Task RevokeFamilyAsync(Guid familyId, string reason, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return context.RefreshTokens
                .Where(t => t.FamilyId == familyId && t.RevokedAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.RevokedAt, now)
                    .SetProperty(t => t.RevokedReason, reason), cancellationToken);
        }

        public Task RevokeAllForUserAsync(Guid userId, string reason, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return context.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.RevokedAt, now)
                    .SetProperty(t => t.RevokedReason, reason), cancellationToken);
        }

        public Task DeleteExpiredAsync(Guid userId, TimeSpan olderThan, CancellationToken cancellationToken = default)
        {
            var limit = DateTime.UtcNow - olderThan;
            return context.RefreshTokens
                .Where(t => t.UserId == userId && t.ExpiresAt < limit)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<TokenSubject?> GetTokenSubjectAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new { u.UserId, u.Email })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return null;

            var roleIds = await context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RolId)
                .ToListAsync(cancellationToken);

            return new TokenSubject(user.UserId, user.Email, roleIds);
        }

        public Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email.Trim();
            return context.Users
                .AsNoTracking()
                .Where(u => u.Email == normalized)
                .Select(u => (Guid?)u.UserId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
