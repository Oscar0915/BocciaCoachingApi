using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace BocciaCoaching.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(InfoUserRegisterDto userDto)
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

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var Athlete = new UserRol
                {
                    RolId = userDto.Rol,
                    UserId = user.UserId
                };

                await _context.UserRols.AddAsync(Athlete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<InfoBasicUserDto>> GetAllAsync()
        {
            try
            {
                var usersData = await _context.Users.ToListAsync();
                var infoUser = usersData.Select(u => new InfoBasicUserDto
                {
                    Name = u.FirstName,
                }).ToList();

                return infoUser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<InfoBasicUserDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
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
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto?> Login(LoginRequestDto loginDto)
        {
            var user = await GetUserByEmailAsync(loginDto.Email);
            if (user == null) return null;

            // Verificar contraseña
            bool validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!validPassword) return null;

            var roles =  _context.UserRols.Where(x=>x.UserId==user.UserId).ToList();


            // Generar respuesta
            return new LoginResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                RolId = roles[0].RolId
            };
        }

        /// <summary>
        /// Registrar atleta
        /// </summary>
        /// <param name="atlheteInfoSave"></param>
        /// <returns></returns>
        public async Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(atlheteInfoSave.Password);

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

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();



                var Athlete = new UserRol
                {
                    RolId = 3,
                    UserId = user.UserId

                };

                await _context.UserRols.AddAsync(Athlete);
                await _context.SaveChangesAsync();




                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return false;
            }
        }

        public async Task<ValidateEmailDto> ValidateEmail(ValidateEmailDto email)
        {
            var isAvailable = await GetUserByEmailAsync(email.Email);

            if (isAvailable != null)
                return new ValidateEmailDto { Email = "No disponible" };

            return new ValidateEmailDto { Email = email.Email };

        }

    }
}
