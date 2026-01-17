using Foodie.Business.Services.Interfaces;
using Foodie.Models.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if(dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                LoginResponseDto response = await _authService.AuthenticateAsync(HttpContext, dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
