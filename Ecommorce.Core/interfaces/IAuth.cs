using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Sharing;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<bool> SendEmailToResetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPassword);
        Task SendEmail(string email, string code, string component, string subject, string message);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> GetUserAddress(string email);
    }
}
