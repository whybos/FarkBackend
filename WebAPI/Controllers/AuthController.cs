using Business.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

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
            try
            {
                var userToLogin = _authService.Login(userForLoginDto);
                if (!userToLogin.Success)
                    return BadRequest(userToLogin);

                var result = _authService.CreateAccessToken(userToLogin.Data);
                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPost("register")]
        [HttpPost("add")]
        [HttpPost("addUser")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                var emailToCheck = !string.IsNullOrWhiteSpace(userForRegisterDto?.email)
                    ? userForRegisterDto.email
                    : userForRegisterDto?.username;

                if (userForRegisterDto == null || string.IsNullOrWhiteSpace(emailToCheck))
                    return BadRequest(new ErrorResult("E-posta adresi boş olamaz."));

                var userExists = _authService.UserExists(emailToCheck);
                if (!userExists.Success)
                    return BadRequest(userExists);

                var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.password);
                if (!registerResult.Success || registerResult.Data == null)
                    return BadRequest(registerResult);

                var result = _authService.CreateAccessToken(registerResult.Data);
                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPost("validate")]
        public ActionResult Validate()
        {
            try
            {
                var result = _authService.Validate();
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _userService.GetAll();
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpDelete("delete/{id}")]
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _userService.Delete(id);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpDelete("resetUsers")]
        [HttpPost("resetUsers")]
        public IActionResult ResetUsers()
        {
            try
            {
                var result = _userService.ResetUsers();
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpDelete("resetDatabase")]
        [HttpPost("resetDatabase")]
        [HttpDelete("reset")]
        [HttpPost("reset")]
        public IActionResult ResetDatabase()
        {
            try
            {
                var result = _userService.ResetDatabase();
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

    }
}
