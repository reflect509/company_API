using API.v1.Models;
using API.v1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.v1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] AppUser request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new ApiError
                {
                    Message = "Некорректные данные",
                    ErrorCode = 1001
                }); 
            }

            var token = await authService.Authenticate(request.UserName, request.UserPassword);

            if (token == null)
            {
                return StatusCode(403, new ApiError
                {
                    Message = "Неверное имя пользователя или пароль",
                    ErrorCode = 1002
                });
            }

            return Ok(new { Token = token });
        }
    }
}
