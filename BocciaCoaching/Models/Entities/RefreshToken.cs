using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// ES: Refresh token emitido a un usuario. Solo se guarda el hash SHA-256 del token,
    /// nunca el valor en claro. Los tokens de una misma sesión comparten <see cref="FamilyId"/>:
    /// si se reutiliza un token ya rotado (posible robo), se revoca toda la familia.
    /// </summary>
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid RefreshTokenId { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        /// <summary>ES: Hash SHA-256 (hex, 64 caracteres) del token entregado al cliente.</summary>
        [Required]
        [MaxLength(64)]
        public string TokenHash { get; set; } = string.Empty;

        /// <summary>ES: Identificador de la sesión (cadena de rotaciones) a la que pertenece el token.</summary>
        public Guid FamilyId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        /// <summary>ES: Fecha de revocación (por rotación, logout o reuso detectado). Null = activo.</summary>
        public DateTime? RevokedAt { get; set; }

        [MaxLength(32)]
        public string? RevokedReason { get; set; }

        /// <summary>ES: Token que reemplazó a este en la rotación.</summary>
        public Guid? ReplacedByTokenId { get; set; }

        [MaxLength(45)]
        public string? CreatedByIp { get; set; }
    }
}
