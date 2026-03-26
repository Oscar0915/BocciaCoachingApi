using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       private readonly IUserService _user;
       private readonly IFileStorageService _fileStorage;


        public UserController(IUserService user, IFileStorageService fileStorage)
        {
            _user = user;
            _fileStorage = fileStorage;
        }

        [HttpGet(Name = "GetInfoUser")]
        public async Task<ActionResult<ResponseContract<IEnumerable<InfoBasicUserDto>>>> Get()
        {
            var response = await _user.GetAllAsync();
            return Ok(response);
        }


        [HttpPost("AddInfoUser")]
        public async Task<ActionResult<ResponseContract<bool>>> Add(InfoUserRegisterDto user)
        {
            var response = await _user.AddUser(user);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> Login(LoginRequestDto user)
        {
            var response = await _user.Login(user);
            return Ok(response);
        }


        [HttpPost("AddAthlete")]
        public async Task<ActionResult<ResponseContract<int>>> NewAthlete(AtlheteInfoSave user)
        {
            var response = await _user.RegistrarAtleta(user);
            return Ok(response);
        }


        [HttpPost("ValidateEmail")]
        public async Task<ActionResult<ResponseContract<ValidateEmailDto>>> ValidateEmail(ValidateEmailDto email)
        {
            var response = await _user.ValidateEmail(email);
            return Ok(response);
        }
        
        
        [HttpPost("SearchAthletesForNameAndTeams")]
        public async Task<ActionResult<ResponseContract<List<AtlheteInfo>>>> SearchAthletesForNameAndTeams(SearchDataAthleteDto email)
        {
            var response = await _user.GetAthleteForName(email);
            return Ok(response);
        }

        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var response = await _user.UpdatePassword(updatePasswordDto);
            return Ok(response);
        }

        [HttpPut("UpdateUserInfo")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            var response = await _user.UpdateUserInfo(updateUserInfoDto);
            return Ok(response);
        }

        
    }
}
