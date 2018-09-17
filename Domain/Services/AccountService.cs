using Data.Models.Entities;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> SignIn(string email, string password, bool rememberMe, bool lockOutPeriod = false)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            return result;
        }

        public async Task<AppUser> FindUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ResetPassword(AppUser user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }

        public async Task<string> GeneratePasswordResetToken(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}