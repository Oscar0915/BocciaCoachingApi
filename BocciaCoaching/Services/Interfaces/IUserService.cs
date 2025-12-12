﻿using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseContract<IEnumerable<InfoBasicUserDto>>> GetAllAsync();
        Task<ResponseContract<InfoBasicUserDto>> GetByIdAsync(int id);
        Task<ResponseContract<bool>> AddUser(InfoUserRegisterDto userDto);
        Task<ResponseContract<LoginResponseDto>> Login(LoginRequestDto loginDto);
        Task<ResponseContract<int>> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave);
      
        Task<ResponseContract<ValidateEmailDto>> ValidateEmail(ValidateEmailDto email);

        /// <summary>
        /// Método para buscar atletas por nombre pertenecientes a un club o equipo
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseContract<List<AtlheteInfo>>> GetAthleteForName(SearchDataAthleteDto user);

    }
}
