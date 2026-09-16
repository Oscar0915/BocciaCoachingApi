using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using FirebaseAdmin.Auth;

namespace BocciaCoaching.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthenticationService _firebaseAuthenticationService;

        public UserService(
            IUserRepository repository,
            INotificationService notificationService,
            IFirebaseAuthenticationService firebaseAuthenticationService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _firebaseAuthenticationService = firebaseAuthenticationService;
        }

        public async Task<ResponseContract<bool>> AddUser(InfoUserRegisterDto userDto)
        {
            return await _repository.AddUser(userDto);
        }

        
        public async Task<ResponseContract<IEnumerable<InfoBasicUserDto>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ResponseContract<InfoBasicUserDto>> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ResponseContract<LoginResponseDto>> Login(LoginRequestDto loginDto)
        {
            return await _repository.Login(loginDto);
        }

        public async Task<ResponseContract<LoginResponseDto>> LoginWithGoogle(FirebaseLoginRequestDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.IdToken))
                return ResponseContract<LoginResponseDto>.Fail("El token de Firebase es requerido");

            try
            {
                var identity = await _firebaseAuthenticationService.VerifyIdTokenAsync(loginDto.IdToken);
                if (!identity.EmailVerified)
                    return ResponseContract<LoginResponseDto>.Fail("El email de Google debe estar verificado");

                return await _repository.LoginWithEmailAsync(identity.Email);
            }
            catch (FirebaseAuthException)
            {
                return ResponseContract<LoginResponseDto>.Fail("El token de Firebase no es válido o ha expirado");
            }
            catch (ArgumentException)
            {
                return ResponseContract<LoginResponseDto>.Fail("El token de Firebase no es válido");
            }
            catch (InvalidOperationException)
            {
                return ResponseContract<LoginResponseDto>.Fail(
                    "La autenticación con Firebase no está configurada correctamente");
            }
        }

        public async Task<ResponseContract<Guid>> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            var result = await _repository.RegistrarAtleta(atlheteInfoSave);
            
            if (result.Success && result.Data != Guid.Empty)
            {
                // Crear notificación de bienvenida para el atleta recién creado
                var notificationMessage = new RequestCreateNotificationMessageDto
                {
                    NotificationTypeId = WellKnownIds.NotificationTypeGeneral,
                    ReceiverId = result.Data,
                    SenderId = atlheteInfoSave.CoachId,
                    Message = "Bienvenido a Boccia Coaching. Tu contraseña por defecto es: boccia123. Por favor, cámbiala en tu primer inicio de sesión.",
                    Status = true
                };
                
                await _notificationService.CreateMessage(notificationMessage);

                // Si se proporciona un TeamId, enviar invitación de equipo
                if (atlheteInfoSave.TeamId.HasValue && atlheteInfoSave.TeamId.Value != Guid.Empty)
                {
                    var teamInvitation = new RequestCreateNotificationMessageDto
                    {
                        NotificationTypeId = WellKnownIds.NotificationTypeTeamInvitation, // Tipo 2 para invitaciones de equipo
                        ReceiverId = result.Data,
                        SenderId = atlheteInfoSave.CoachId,
                        Message = "Has sido invitado a unirte al equipo. ¡Acepta la invitación para formar parte del equipo!",
                        ReferenceId = atlheteInfoSave.TeamId.Value,
                        Status = true
                    };
                    
                    await _notificationService.CreateMessage(teamInvitation);
                }
            }
            
            return result;
        }


        public async Task<ResponseContract<ValidateEmailDto>> ValidateEmail(ValidateEmailDto email)
        {
            return await _repository.ValidateEmail(email);
        }

        public async Task<ResponseContract<List<AtlheteInfo>>> GetAthleteForName(SearchDataAthleteDto user)
        {
            var responseInfoAthletes = new List<AtlheteInfo>();
            var dataAthletes= await _repository.GetUserForName(user);

            if (dataAthletes.Success && dataAthletes.Data != null && dataAthletes.Data.Count > 0)
            {
                foreach (var atlhete in dataAthletes.Data)
                {
                    AtlheteInfo atlheteInfo = new AtlheteInfo();
                    atlheteInfo.AthleteId = atlhete.UserId;
                    atlheteInfo.Name = atlhete.FirstName  + " " + atlhete.LastName;
                    responseInfoAthletes.Add(atlheteInfo);
                }
                
                return ResponseContract<List<AtlheteInfo>>.Ok(
                    responseInfoAthletes,
                    "Búsqueda realizada satisfactoriamente"
                );
            }
            
            return ResponseContract<List<AtlheteInfo>>.Fail("No se encontraron atletas");
        }

        public async Task<ResponseContract<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto.UserId == Guid.Empty && string.IsNullOrWhiteSpace(updatePasswordDto.Email))
                return ResponseContract<bool>.Fail("Debe proporcionar el usuario o el email.");

            if (!string.IsNullOrWhiteSpace(updatePasswordDto.Email))
                updatePasswordDto.Email = updatePasswordDto.Email.Trim();

            return await _repository.UpdatePassword(updatePasswordDto);
        }

        public async Task<ResponseContract<bool>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            return await _repository.UpdateUserInfo(updateUserInfoDto);
        }

        public async Task<ResponseContract<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordDto.Email))
                return ResponseContract<bool>.Fail("El email es requerido");

            resetPasswordDto.Email = resetPasswordDto.Email.Trim();

            return await _repository.ResetPassword(resetPasswordDto);
        }

        public async Task<ResponseContract<string?>> UpdateUserImageAsync(Guid userId, string imageUrl)
        {
            return await _repository.UpdateUserImageAsync(userId, imageUrl);
        }
    }
}
