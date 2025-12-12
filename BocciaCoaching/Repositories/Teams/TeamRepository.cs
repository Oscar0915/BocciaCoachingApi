﻿using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Teams
{
    public class TeamRepository(ApplicationDbContext context) : ITeamRepository
    {
        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        public async Task<ResponseContract<int>> AddTeam(RequestTeamDto requestTeamDto)
        {
            try
            {
                var team = new Team()
                {
                    CoachId = requestTeamDto.CoachId,
                    NameTeam = requestTeamDto.NameTeam,
                    Description = requestTeamDto.Description,
                };

                await context.Teams.AddAsync(team);
                await context.SaveChangesAsync();

                // Relacionar automáticamente al entrenador con el equipo
                var teamUser = new TeamUser()
                {
                    TeamId = team.TeamId,
                    UserId = requestTeamDto.CoachId,
                    DateCreation = DateTime.Now
                };

                await context.TeamsUsers.AddAsync(teamUser);
                await context.SaveChangesAsync();

                return ResponseContract<int>.Ok(team.TeamId, "Equipo creado exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<int>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamMemberDto"></param>
        /// <returns></returns>
        public async Task<ResponseContract<bool>> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            try
            {
                var teamUser = new TeamUser()
                {
                    TeamId = requestTeamMemberDto.TeamId,
                    UserId = requestTeamMemberDto.UserId,
                    DateCreation = DateTime.Now,
                };

                await context.TeamsUsers.AddAsync(teamUser);
                await context.SaveChangesAsync();
                
                return ResponseContract<bool>.Ok(true, "Miembro agregado exitosamente al equipo");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail(ex.Message);
            }

        }
        /// <summary>
        /// Método para obtener los equipos al que pertenece los entrenadores 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Team>> GetTeamsForUser(RequestTeamDto requestTeamDto)
        {
            try
            {
                var teamUsers = await context.TeamsUsers
                    .Where(tu => tu.UserId == requestTeamDto.CoachId && tu.Team != null)
                    .Include(tu => tu.Team)
                    .ToListAsync();

                return teamUsers.Where(tu => tu.Team != null).Select(tu => tu.Team!).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetTeamsForUserAsync: {ex.Message}");
                return new List<Team>(); // Retorna lista vacía si hay error
            }
        }
        /// <summary>
        /// Método para obtener los integrantes de un equipo
        /// </summary>
        /// <param name="requestGetUserForTeamDto"></param>
        /// <returns></returns>
        public async Task<ResponseContract<List<User>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            try
            {
                var teamUsers = await context.TeamsUsers
                .Where(tu => tu.TeamId == requestGetUserForTeamDto.TeamId && tu.User != null)
                .Include(tu => tu.User!)
                    .ThenInclude(u => u.UserRoles!)
                    .ThenInclude(ru => ru.Rol)
                .ToListAsync();

                var users = teamUsers
                    .Where(tu => tu.User != null && 
                           tu.User.UserRoles != null && 
                           tu.User.UserRoles.Any(ru => ru.RolId == requestGetUserForTeamDto.RolId))
                    .Select(tu => tu.User!)
                    .ToList();

                return ResponseContract<List<User>>.Ok(users, "Ususarios obtenidos satisfactoriamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUsersForTeam: {ex.Message}");
                return ResponseContract<List<User>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Método para actualizar la imagen del equipo
        /// </summary>
        /// <param name="requestUpdateImageTeamDto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto)
        {
            try
            {
                var team = await context.Teams.FirstOrDefaultAsync(t => t.TeamId == requestUpdateImageTeamDto.TeamId);

                if (team == null)
                    return false;

                if (!string.IsNullOrEmpty(requestUpdateImageTeamDto.Image))
                {
                    // Validar que la cadena base64 sea válida
                    if (string.IsNullOrWhiteSpace(requestUpdateImageTeamDto.Image))
                        throw new ArgumentException("La imagen no puede estar vacía.");

                    // Si quieres validar que realmente sea base64
                    if (!IsBase64String(requestUpdateImageTeamDto.Image))
                        throw new ArgumentException("El formato de imagen no es válido.");

                    team.Image = requestUpdateImageTeamDto.Image;
                }

                team.Bc1 = requestUpdateImageTeamDto.Bc1;
                team.Bc2 = requestUpdateImageTeamDto.Bc2;
                team.Bc3 = requestUpdateImageTeamDto.Bc3;
                team.Bc4 = requestUpdateImageTeamDto.Bc4;

                team.Country = requestUpdateImageTeamDto.Country;
                team.Region = requestUpdateImageTeamDto.Region;
                
                team.UpdatedAt = DateTime.UtcNow;

                context.Teams.Update(team);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si tienes un sistema de auditoría
                Console.WriteLine($"Error al actualizar la imagen: {ex.Message}");
                return false;
            }
        }
        private bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }



    }
}
