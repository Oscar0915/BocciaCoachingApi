﻿﻿﻿using BocciaCoaching.Data;
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
                    Dni = u.Dni,
                    FirstName = u.FirstName ?? string.Empty,
                    LastName = u.LastName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    Password = u.Password ?? string.Empty,
                    Address = u.Address,
                    Country = u.Country,
                    Image = u.Image,
                    Category = u.Category,
                    Seniority = u.Seniority,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
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
                    Dni = user.Dni,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    Country = user.Country,
                    Image = user.Image,
                    Category = user.Category,
                    Seniority = user.Seniority,
                    Status = user.Status,
                    RolId = roles[0].RolId,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
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
                    return ResponseContract<ValidateEmailDto>.Fail(
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
        /// Obtener usuario por correo electrónico
        /// </summary>
        public async Task<ResponseContract<User>> GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return ResponseContract<User>.Fail("El email es requerido");

                var user = await GetUserByEmailAsync(email);
                
                if (user == null)
                    return ResponseContract<User>.Fail("Usuario no encontrado con ese email");

                return ResponseContract<User>.Ok(user, "Usuario encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUserByEmail: {ex.Message}");
                return ResponseContract<User>.Fail($"Error al buscar usuario: {ex.Message}");
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

        public async Task<ResponseContract<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                // Buscar el usuario por ID
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == updatePasswordDto.UserId);
                
                if (user == null)
                {
                    return ResponseContract<bool>.Fail("Usuario no encontrado");
                }

                // Verificar la contraseña actual
                bool validCurrentPassword = BCrypt.Net.BCrypt.Verify(updatePasswordDto.CurrentPassword, user.Password);
                
                if (!validCurrentPassword)
                {
                    return ResponseContract<bool>.Fail("La contraseña actual es incorrecta");
                }

                // Hashear la nueva contraseña
                string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);
                
                // Actualizar la contraseña
                user.Password = newPasswordHash;
                user.UpdatedAt = DateTime.Now;
                
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Contraseña actualizada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdatePassword: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al actualizar la contraseña: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            try
            {
                // Buscar el usuario por ID
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == updateUserInfoDto.UserId);
                
                if (user == null)
                {
                    return ResponseContract<bool>.Fail("Usuario no encontrado");
                }

                // Validar que el DNI sea único si se está actualizando
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Dni) && updateUserInfoDto.Dni != user.Dni)
                {
                    var existingUserWithDni = await context.Users
                        .FirstOrDefaultAsync(u => u.Dni == updateUserInfoDto.Dni && u.UserId != updateUserInfoDto.UserId);
                    
                    if (existingUserWithDni != null)
                    {
                        return ResponseContract<bool>.Fail("Ya existe un usuario con ese DNI");
                    }
                }

                // Validar que el email sea único si se está actualizando
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Email) && updateUserInfoDto.Email != user.Email)
                {
                    var existingUserWithEmail = await context.Users
                        .FirstOrDefaultAsync(u => u.Email == updateUserInfoDto.Email && u.UserId != updateUserInfoDto.UserId);
                    
                    if (existingUserWithEmail != null)
                    {
                        return ResponseContract<bool>.Fail("Ya existe un usuario con ese email");
                    }
                }

                // Actualizar solo los campos que no sean nulos
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Dni))
                    user.Dni = updateUserInfoDto.Dni;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.FirstName))
                    user.FirstName = updateUserInfoDto.FirstName;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.LastName))
                    user.LastName = updateUserInfoDto.LastName;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Email))
                    user.Email = updateUserInfoDto.Email;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Address))
                    user.Address = updateUserInfoDto.Address;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Country))
                    user.Country = updateUserInfoDto.Country;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Image))
                    user.Image = updateUserInfoDto.Image;
                
                if (!string.IsNullOrWhiteSpace(updateUserInfoDto.Category))
                    user.Category = updateUserInfoDto.Category;
                
                if (updateUserInfoDto.Seniority.HasValue)
                    user.Seniority = updateUserInfoDto.Seniority;
                
                if (updateUserInfoDto.Status.HasValue)
                    user.Status = updateUserInfoDto.Status;

                // Actualizar fecha de modificación
                user.UpdatedAt = DateTime.Now;
                
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Información de usuario actualizada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateUserInfo: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al actualizar la información del usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtener usuario directamente por ID (para uso en servicios)
        /// </summary>
        public async Task<User?> GetUserEntityByIdAsync(int id)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUserEntityByIdAsync: {ex.Message}");
                return null;
            }
        }
    }
}
