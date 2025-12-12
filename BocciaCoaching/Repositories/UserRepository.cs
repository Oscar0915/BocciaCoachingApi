﻿using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<ResponseContract<bool>> AddUser(InfoUserRegisterDto userDto)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var user = new User
                {
                    Email = userDto.Email,
                    Password = passwordHash,
                    Country = userDto.Region,
                    FirstName = "Name",
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var athlete = new UserRol
                {
                    RolId = userDto.Rol,
                    UserId = user.UserId
                };

                await context.UserRoles.AddAsync(athlete);
                await context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Usuario creado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al crear usuario: {ex.Message}");
            }
        }

        public async Task<ResponseContract<IEnumerable<InfoBasicUserDto>>> GetAllAsync()
        {
            try
            {
                var usersData = await context.Users.ToListAsync();
                var infoUser = usersData.Select(u => new InfoBasicUserDto
                {
                    Name = u.FirstName
                }).ToList();

                return ResponseContract<IEnumerable<InfoBasicUserDto>>.Ok(infoUser, "Usuarios obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAllAsync: {ex.Message}");
                return ResponseContract<IEnumerable<InfoBasicUserDto>>.Fail($"Error al obtener usuarios: {ex.Message}");
            }
        }

        public async Task<ResponseContract<InfoBasicUserDto>> GetByIdAsync(int id)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                
                if (user == null)
                    return ResponseContract<InfoBasicUserDto>.Fail("Usuario no encontrado");

                var infoUser = new InfoBasicUserDto
                {
                    Name = user.FirstName
                };

                return ResponseContract<InfoBasicUserDto>.Ok(infoUser, "Usuario obtenido exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetByIdAsync: {ex.Message}");
                return ResponseContract<InfoBasicUserDto>.Fail($"Error al obtener usuario: {ex.Message}");
            }
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users
     .Select(u => new User
     {
         UserId = u.UserId,
         Email = u.Email ?? string.Empty,
         Password = u.Password ?? string.Empty,
         FirstName = u.FirstName ?? string.Empty,
         LastName = u.LastName ?? string.Empty
     })
     .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Método para iniciar sesión 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseContract<LoginResponseDto>> Login(LoginRequestDto loginDto)
        {
            try
            {
                var user = await GetUserByEmailAsync(loginDto.Email);
                if (user == null) 
                    return ResponseContract<LoginResponseDto>.Fail("Usuario no encontrado");

                // Verificar contraseña
                bool validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
                if (!validPassword) 
                    return ResponseContract<LoginResponseDto>.Fail("Contraseña incorrecta");

                var roles = context.UserRoles.Where(x => x.UserId == user.UserId).ToList();

                if (roles.Count == 0)
                    return ResponseContract<LoginResponseDto>.Fail("Usuario sin roles asignados");

                // Generar respuesta
                var response = new LoginResponseDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    RolId = roles[0].RolId
                };

                return ResponseContract<LoginResponseDto>.Ok(response, "Login exitoso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Login: {ex.Message}");
                return ResponseContract<LoginResponseDto>.Fail($"Error al iniciar sesión: {ex.Message}");
            }
        }

        /// <summary>
        /// Registrar atleta
        /// </summary>
        /// <param name="atlheteInfoSave"></param>
        /// <returns></returns>
        public async Task<ResponseContract<int>> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            try
            {
                // Contraseña por defecto
                string defaultPassword = "boccia123";
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

                var user = new User
                {
                    FirstName = atlheteInfoSave.FirstName,
                    Email = atlheteInfoSave.Email,
                    Dni = atlheteInfoSave.Dni,
                    LastName = atlheteInfoSave.LastName,
                    Address = atlheteInfoSave.Address,
                    Seniority = atlheteInfoSave.Seniority,
                    Status = atlheteInfoSave.Status,
                    CreatedAt = DateTime.Now,
                    Password = passwordHash,
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var athlete = new UserRol
                {
                    RolId = 3,
                    UserId = user.UserId
                };

                await context.UserRoles.AddAsync(athlete);
                await context.SaveChangesAsync();

                return ResponseContract<int>.Ok(user.UserId, "Atleta registrado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegistrarAtleta: {ex.Message}");
                return ResponseContract<int>.Fail($"Error al registrar atleta: {ex.Message}");
            }
        }

        public async Task<ResponseContract<ValidateEmailDto>> ValidateEmail(ValidateEmailDto email)
        {
            try
            {
                var isAvailable = await GetUserByEmailAsync(email.Email);

                if (isAvailable != null)
                    return ResponseContract<ValidateEmailDto>.Ok(
                        new ValidateEmailDto { Email = "No disponible" }, 
                        "Email no disponible"
                    );

                return ResponseContract<ValidateEmailDto>.Ok(
                    new ValidateEmailDto { Email = email.Email }, 
                    "Email disponible"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ValidateEmail: {ex.Message}");
                return ResponseContract<ValidateEmailDto>.Fail($"Error al validar email: {ex.Message}");
            }
        }

        /// <summary>
        /// Método para la busqueda de los atletas por nombre
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseContract<List<User>>> GetUserForName(SearchDataAthleteDto user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user?.FirstName))
                    return ResponseContract<List<User>>.Fail("Debe proporcionar un nombre para la búsqueda.");

                var userInfo = await (from u in context.Users
                        join tu in context.TeamsUsers on u.UserId equals tu.UserId
                        join ur in context.UserRoles on u.UserId equals ur.UserId
                        where u.FirstName != null
                              && EF.Functions.Like(u.FirstName, $"%{user.FirstName}%")
                              && tu.TeamId == user.TeamId
                              && ur.RolId == 3
                        select u)
                    .Distinct()
                    .ToListAsync();


                return ResponseContract<List<User>>.Ok(userInfo, "Búsqueda realizada satisfactoriamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<User>>.Fail("Ocurrió un error realizando la búsqueda");
            }
        }
    }
}
