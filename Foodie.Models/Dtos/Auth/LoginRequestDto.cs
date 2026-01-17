using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Dtos.Auth
{
    /// <summary>
    /// DTO for user login requests
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// The email address of the user
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        /// <summary>
        /// The password of the user attempting to log in
        /// </summary>
        [Required]
        public string Password { get; set; } = default!;

    }
}
