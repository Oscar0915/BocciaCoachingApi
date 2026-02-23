﻿﻿using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;

namespace BocciaCoaching.Repositories.Interfaces.ITeams
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        Task<ResponseContract<int>> AddTeam(RequestTeamDto requestTeamDto);


        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamMemberDto"></param>
        /// <returns></returns>
        Task<ResponseContract<bool>> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto);

        /// <summary>
        /// Método para traer el listado de equipos relacionados a un coach con la cantidad de integrantes por equipo
        /// </summary>
        /// <param name="coachId">Identificador del coach</param>
        /// <returns></returns>
        Task<ResponseContract<List<TeamSummaryDto>>> GetTeamsForUser(int coachId);

        Task<ResponseContract<List<TeamMemberDto>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto);
        Task<bool> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto);
        
        /// <summary>
        /// Verificar si un usuario ya pertenece a un equipo
        /// </summary>
        Task<bool> IsUserInTeam(int userId, int teamId);

        /// <summary>
        /// Contar la cantidad de equipos que ha creado un usuario
        /// </summary>
        Task<int> CountTeamsByUserIdAsync(int userId);

        /// <summary>
        /// Contar la cantidad de atletas en un equipo
        /// </summary>
        Task<int> CountAthletesByTeamIdAsync(int teamId);
    }
}
