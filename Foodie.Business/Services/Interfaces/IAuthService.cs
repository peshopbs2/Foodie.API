using Foodie.Models.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Interfaces
{
    /// <summary>
    /// Defines methods for user authentication
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using email and password
        /// </summary>
        /// <returns>JWT and refresh tokens</returns>
        Task<LoginResponseDto> AuthenticateAsync(HttpContext context, LoginRequestDto dto);

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="dto">User data</param>
        /// <returns>The registered user object</returns>
        Task<UserResponseDto> RegisterUserAsync(RegisterUserRequestDto dto);
    }
}
