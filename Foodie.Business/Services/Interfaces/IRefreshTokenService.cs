using Foodie.Models.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateAndStoreRefreshTokenAsync(IdentityUser user);

        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);

        Task InvalidateRefreshTokenAsync(string refreshToken);

        Task<Guid?> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}
