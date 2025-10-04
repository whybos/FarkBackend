using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
                return BadRequest(userToLogin.Message);

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.email);
            if (!userExists.Success)
                return BadRequest(userExists.Message);

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
        }

        [HttpPost("validate")]
        public ActionResult Validate()
        {
            var result = _authService.Validate();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getAll")]
        [SecuredOperation("superAdmin")]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [SecuredOperation("superAdmin")] // sadece super admin
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
