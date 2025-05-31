using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.DTO
{
    public record LoginDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }

    }
    public record RegisterDTO:LoginDTO
    {
        public string UserName { get; set; }
 
        public string? DisplayName { get; set; } // ← أضف هذا

    }
    public record ResetPasswordDTO : LoginDTO
    {
        public  string  Token { get; set; }
    }
    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }

    }
}
