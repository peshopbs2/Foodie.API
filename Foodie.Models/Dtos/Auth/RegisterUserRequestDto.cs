using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Dtos.Auth
{
    /// <summary>
    /// DTO for user registration
    /// </summary>
    public class RegisterUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
    }
}
