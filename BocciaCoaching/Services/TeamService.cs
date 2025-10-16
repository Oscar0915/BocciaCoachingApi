﻿using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeam(RequestTeamDto requestTeamDto)
        {
            return await _teamRepository.AddTeam(requestTeamDto);
        }

        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del mienbro equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            return await _teamRepository.AddTeamMember(requestTeamMemberDto);
        }


        public async Task<List<Team>> GetTeamsForUser(int idUser)
        {
            return  await _teamRepository.GetTeamsForUser(idUser);
        }

        public async Task<List<User>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            return await _teamRepository.GetUsersForTeam(requestGetUserForTeamDto);
        }



    }
}
