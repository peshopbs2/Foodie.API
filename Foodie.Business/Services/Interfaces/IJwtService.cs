using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Interfaces
{
    public interface IJwtService
    {
        public Task<string> GenerateTokenAsync(IdentityUser user);
    }
}
