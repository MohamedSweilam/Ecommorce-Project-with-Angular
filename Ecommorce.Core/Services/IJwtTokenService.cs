using Ecommorce.Core.Entities.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.Services
{
    public interface IJwtTokenService
    {
        string GenerateAndSetToken(AppUser user);
    }
}
