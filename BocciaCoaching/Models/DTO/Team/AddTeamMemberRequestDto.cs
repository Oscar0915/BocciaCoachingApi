using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Team;

/// <summary>
/// ES: Cuerpo de <c>POST /api/v1/teams/{teamId}/members</c>. El equipo viene en la ruta.
/// </summary>
public class AddTeamMemberRequestDto
{
    [Required]
    public Guid UserId { get; set; }
}
