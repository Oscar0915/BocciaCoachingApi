using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("User")]
    public class User: IAuditable
    {
        /// <summary>
        /// Campo: Identificador del usuario
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// Campo: Numero de identificación 
        /// Field: 
        /// </summary>
        public string? Dni { get; set; }
        /// <summary>
        /// Campo: Nombres del usuario
        /// Field: 
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Campo: Apellidos del usuario
        /// Field: 
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Campo: Correo del usuario
        /// Field: 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Campo: Contraseña del usuario
        /// Field: 
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Campo: Dirección del usuario
        /// Field: 
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Campo: Pais del usuario
        /// Field: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// Campo: Foto de perfil del usuario
        /// Field: 
        /// </summary>
        public string? Image {get; set; }
        /// <summary>
        /// Campo: Categoría del usuario
        /// Field: 
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// Campo: Tiempo en Boccia
        /// Field: 
        /// </summary>
        public DateTime? Seniority { get; set; }
        /// <summary>
        /// Campo: Estado del usuario
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;

        public ICollection<UserRol>? UserRoles { get; set; }
        /// <summary>
        /// Campo: Sesion activa
        /// Field: 
        /// </summary>
        public Session? Session { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
