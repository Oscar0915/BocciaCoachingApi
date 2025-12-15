using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;
        private readonly INotificationService _notificationService;

        public UserService(IUserRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<ResponseContract<bool>> AddUser(InfoUserRegisterDto userDto)
        {
            return await _repository.AddUser(userDto);
        }

        
        public async Task<ResponseContract<IEnumerable<InfoBasicUserDto>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ResponseContract<InfoBasicUserDto>> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ResponseContract<LoginResponseDto>> Login(LoginRequestDto loginDto)
        {
            return await _repository.Login(loginDto);
        }

        public async Task<ResponseContract<int>> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            var result = await _repository.RegistrarAtleta(atlheteInfoSave);
            
            if (result.Success && result.Data > 0)
            {
                // Crear notificación de bienvenida para el atleta recién creado
                var notificationMessage = new RequestCreateNotificationMessageDto
                {
                    NotificationTypeId = 1,
                    ReceiverId = result.Data,
                    SenderId = atlheteInfoSave.CoachId,
                    Message = "Bienvenido a Boccia Coaching. Tu contraseña por defecto es: boccia123. Por favor, cámbiala en tu primer inicio de sesión.",
                    Status = true
                };
                
                await _notificationService.CreateMessage(notificationMessage);

                // Si se proporciona un TeamId, enviar invitación de equipo
                if (atlheteInfoSave.TeamId.HasValue && atlheteInfoSave.TeamId.Value > 0)
                {
                    var teamInvitation = new RequestCreateNotificationMessageDto
                    {
                        NotificationTypeId = 2, // Tipo 2 para invitaciones de equipo
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
            return await _repository.UpdatePassword(updatePasswordDto);
        }

        public async Task<ResponseContract<bool>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            return await _repository.UpdateUserInfo(updateUserInfoDto);
        }
    }
}
