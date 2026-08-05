using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Rol
{
    /// <summary>
    /// ES: Datos para crear un nuevo rol.
    /// EN: Data to create a new role.
    /// </summary>
    public class CreateRolDto
    {
        /// <summary>ES: Descripción / nombre del rol.</summary>
        [Required(ErrorMessage = "La descripción del rol es obligatoria.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La descripción debe tener entre 2 y 100 caracteres.")]
        public required string Description { get; set; }
    }
}

