using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.User
{
    public class UpdateUserInfoDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UserId { get; set; }

        [StringLength(20, ErrorMessage = "El DNI no puede tener más de 20 caracteres")]
        public string? Dni { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(255, ErrorMessage = "El email no puede tener más de 255 caracteres")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "El país no puede tener más de 100 caracteres")]
        public string? Country { get; set; }

        [StringLength(500, ErrorMessage = "La URL de la imagen no puede tener más de 500 caracteres")]
        public string? Image { get; set; }

        [StringLength(50, ErrorMessage = "La categoría no puede tener más de 50 caracteres")]
        public string? Category { get; set; }

        public DateTime? Seniority { get; set; }

        public bool? Status { get; set; }
    }
}
