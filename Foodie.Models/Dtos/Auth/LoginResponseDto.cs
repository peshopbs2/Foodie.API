using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Dtos.Auth
{
    /// <summary>
    /// DTO representing the response returned after success
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets the issued token
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// Gets or sets the refresh token
        /// </summary>
        public string RefreshToken { get; set; } = default!;
    }
}
