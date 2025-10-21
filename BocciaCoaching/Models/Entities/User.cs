using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("User")]
    public class User: IAuditable
    {
        /// <summary>
        /// ES: Identificador del usuario
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// ES: Numero de identificación 
        /// EN: 
        /// </summary>
        public string? Dni { get; set; }
        /// <summary>
        /// ES: Nombres del usuario
        /// EN: 
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// ES: Apellidos del usuario
        /// EN: 
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// ES: Correo del usuario
        /// EN: 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// ES: Contraseña del usuario
        /// EN: 
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// ES: Dirección del usuario
        /// EN: 
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// ES: Pais del usuario
        /// EN: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// ES: Foto de perfil del usuario
        /// EN: 
        /// </summary>
        public string? Image {get; set; }
        /// <summary>
        /// ES: Categoría del usuario
        /// EN: 
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// ES: Tiempo en Boccia
        /// EN: 
        /// </summary>
        public DateTime? Seniority { get; set; }
        /// <summary>
        /// ES: Estado del usuario
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;

        public ICollection<UserRol>? UserRoles { get; set; }
        /// <summary>
        /// ES: Sesion activa
        /// EN: 
        /// </summary>
        public Session? Session { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
