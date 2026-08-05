using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Rol
{
    /// <summary>
    /// ES: Datos para actualizar un rol existente.
    /// EN: Data to update an existing role.
    /// </summary>
    public class UpdateRolDto
    {
        /// <summary>ES: Identificador del rol a actualizar.</summary>
        [Required(ErrorMessage = "El identificador del rol es obligatorio.")]
        public Guid RolId { get; set; }

        /// <summary>ES: Nueva descripción / nombre del rol.</summary>
        [Required(ErrorMessage = "La descripción del rol es obligatoria.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La descripción debe tener entre 2 y 100 caracteres.")]
        public required string Description { get; set; }
    }
}

