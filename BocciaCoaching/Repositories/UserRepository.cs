using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
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
                    FirstName = userDto.FirstName,
                    Email = userDto.Email,
                    Password = passwordHash,
                };

                await _context.Users.AddAsync(user);
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
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Método para iniciar sesión 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto?> IniciarSesion(LoginRequestDto loginDto)
        {
            var user = await GetUserByEmailAsync(loginDto.Email);
            if (user == null) return null;

            // Verificar contraseña
            bool validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!validPassword) return null;

            // Generar respuesta
            return new LoginResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
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

        public async Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var assessStrength = new AssessStrength
                {
                    EvaluationDate = DateTime.Now,
                };

                await _context.AssessStrengths.AddAsync(assessStrength);
                await _context.SaveChangesAsync();

                var response = new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = assessStrength.AssessStrengthId,
                    DateEvaluation = assessStrength.EvaluationDate,
                    State = true
                };



                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = 0,
                    DateEvaluation = DateTime.Now,
                    State = false
                };
            }
        }

        public async Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            try
            {
                var athletesInfo = new AthletesToEvaluated
                {
                    AthleteId = athletesToEvaluated.AthleteId,
                    CoachId = athletesToEvaluated.CoachId,
                    AssessStrengthId = athletesToEvaluated.AssessStrengthId,    
                };
                await _context.AthletesToEvaluated.AddAsync(athletesInfo);
                await _context.SaveChangesAsync();
                return athletesInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return new AthletesToEvaluated
                {
                   AssessStrengthId = 0,
                   AthleteId = 0,
                   CoachId = 0,
                };
            }
        }


        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            try
            {
                var athletesInfo = new EvaluationDetailStrength
                {
                   AssessStrengthId = requestAddDetailToEvaluationForAthlete.AssessStrengthId,
                   AthleteId= requestAddDetailToEvaluationForAthlete .AthleteId,
                   BoxNumber = requestAddDetailToEvaluationForAthlete.BoxNumber,
                   Observations = requestAddDetailToEvaluationForAthlete.Observations,
                   ScoreObtained = requestAddDetailToEvaluationForAthlete.ScoreObtained,
                   Status = requestAddDetailToEvaluationForAthlete.Status,
                   TargetDistance = requestAddDetailToEvaluationForAthlete.TargetDistance,
                   ThrowOrder = requestAddDetailToEvaluationForAthlete.ThrowOrder
                   
                };
                await _context.EvaluationDetailStrengths.AddAsync(athletesInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return false;
            }
        }

    }
}
