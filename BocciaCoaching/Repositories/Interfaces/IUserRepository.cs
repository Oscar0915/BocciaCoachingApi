﻿﻿﻿using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ResponseContract<IEnumerable<InfoBasicUserDto>>> GetAllAsync();
        Task<ResponseContract<InfoBasicUserDto>> GetByIdAsync(int id);
        Task<User?> GetUserEntityByIdAsync(int id); // Método que devuelve directamente la entidad
        Task<ResponseContract<bool>> AddUser(InfoUserRegisterDto userDto);
        Task<ResponseContract<LoginResponseDto>> Login(LoginRequestDto loginDto);
        Task<ResponseContract<int>> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave);
        Task<ResponseContract<ValidateEmailDto>> ValidateEmail(ValidateEmailDto email);

        /// <summary>
        /// Obtener usuario por correo electrónico
        /// </summary>
        Task<ResponseContract<User>> GetUserByEmail(string email);

        /// <summary>
        /// Método para la busqueda de los atletas por nombre
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseContract<List<User>>> GetUserForName(SearchDataAthleteDto user);

        /// <summary>
        /// Actualizar la contraseña de un usuario
        /// </summary>
        /// <param name="updatePasswordDto"></param>
        /// <returns></returns>
        Task<ResponseContract<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto);

        /// <summary>
        /// Actualizar la información de un usuario
        /// </summary>
        /// <param name="updateUserInfoDto"></param>
        /// <returns></returns>
        Task<ResponseContract<bool>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto);
    }
}
