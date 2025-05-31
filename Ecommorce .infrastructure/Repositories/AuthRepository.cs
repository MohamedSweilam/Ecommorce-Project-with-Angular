using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce.Core.Sharing;
using Ecommorce_.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signIn;
        private readonly IGenerateToken generateToken;
        private readonly ApplicationDbContext _context;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signIn, IGenerateToken token, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signIn = signIn;
            this.generateToken = token;
            _context = context;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "This UserName Is Already Registered";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "This Email Is Already Registered";
            }
            AppUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName

            };
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            //send active email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please Activte Your Email");
            return "Done";
        }
        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email,
                "muhamed.sweilam@gmail.com",
                subject,
                EmailStringBody.Send(email, code, component, message));
            await emailService.SendEmail(result);
        }
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return null;
            }
            var findUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (!findUser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please Activte Your Email");
                return "Please Activate Your Email First, Email Confirmation Has Been Sent";

            }
            var result=await signIn.CheckPasswordSignInAsync(findUser, loginDTO.Password, true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(findUser);
            }
            return "Please Check Your Email Or Password Is Incorrect";
            
        }
        public async Task<bool> SendEmailToResetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if(findUser is null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "ResetPassword", "Reset Your Password");
            return true;
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var findUser = await userManager.FindByEmailAsync(resetPassword.Email);
            if(findUser is null) { return null; }
            var result = await userManager.ResetPasswordAsync(findUser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return "Password Has Been Changed";
            }
            return result.Errors.ToList()[0].Description;

        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);

            if (findUser is null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (result.Succeeded)
            {
                return true;
            }


            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please Activte Your Email");
            return false;
        }

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null) return false;
            var myAddress = await _context.Addresses.FirstOrDefaultAsync(m => m.AppUserId == findUser.Id);
            if (myAddress is null)
            {
                await _context.AddAsync(address);
            }
            else
            {
                address.AppUserId = findUser.Id;
                address.Id = myAddress.Id;
                _context.Addresses.Update(address);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> GetUserAddress(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var address = await _context.Addresses.FirstOrDefaultAsync(m => m.AppUserId == user.Id);
            return address;
        }
    }
}
