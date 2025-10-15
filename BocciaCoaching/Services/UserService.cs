﻿using BocciaCoaching.Models.DTO;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddUser(InfoUserRegisterDto userDto)
        {
            return await _repository.AddUser(userDto);
        }

        
        public async Task<IEnumerable<InfoBasicUserDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<InfoBasicUserDto?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<LoginResponseDto?> Login(LoginRequestDto loginDto)
        {
            return _repository.Login(loginDto);
        }

        public Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            return _repository.RegistrarAtleta(atlheteInfoSave);
        }


        public Task<ValidateEmailDto> ValidateEmail(ValidateEmailDto email)
        {
            return _repository.ValidateEmail(email);
        }
    }
}
