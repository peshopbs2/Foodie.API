using Foodie.Business.Repositories.Interfaces;
using Foodie.Business.Services.Interfaces;
using Foodie.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private IRepository<RefreshToken> _refreshTokenRepository;
        public RefreshTokenService(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GenerateAndStoreRefreshTokenAsync(IdentityUser user)
        {
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            RefreshToken refreshToken = new RefreshToken()
            {
                Token = token,
                UserId = Guid.Parse(user.Id),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.CommitAsync();
            return token;
        }

        public async Task<Guid?> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.Query()
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsRevoked == false);
            if(token != null)
            {
                return token.UserId;
            }

            return null;
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            RefreshToken? token = await _refreshTokenRepository.Query()
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if(token != null)
            {
                token.IsRevoked = true;
                await _refreshTokenRepository.CommitAsync();
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var token = await _refreshTokenRepository.Query()
               .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsRevoked == false && rt.UserId == userId);
            return token != null;
        }
    }
}
