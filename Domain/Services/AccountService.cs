using System.Security.Claims;
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

        /// <summary>
        /// Used for user sign-in
        /// </summary>
        /// <param name="email">holds the user email address</param>
        /// <param name="password">holds the user password</param>
        /// <param name="rememberMe">holds the remember me data</param>
        /// <param name="lockOutPeriod">holds the lockout period</param>
        /// <returns>SignInResult</returns>
        public async Task<SignInResult> SignIn(string email, string password, bool rememberMe, bool lockOutPeriod = false)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        /// <summary>
        /// Used to sign-out user
        /// </summary>
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Used to Register a user
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="password">holds the user password</param>
        /// <returns>IndentityResult</returns>
        public async Task<IdentityResult> Register(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            return result;
        }

        /// <summary>
        /// Used to find user by email
        /// </summary>
        /// <param name="email">Holds the user email</param>
        /// <returns>User data</returns>
        public async Task<AppUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Used to reset user password
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="code">holds the user code</param>
        /// <param name="password">holds the user password</param>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> ResetPassword(AppUser user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }

        /// <summary>
        /// Generates reset password token
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <returns>Reset Token</returns>
        public async Task<string> GeneratePasswordResetToken(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <summary>
        /// Sets User region
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="id">holds the region id</param>
        public async Task SetUserRegion(ClaimsPrincipal user, int? id)
        {
            var currentUser = await _userManager.GetUserAsync(user);

            currentUser.RegionId = id;
            await _userManager.UpdateAsync(currentUser);
        }

        /// <summary>
        /// Used to find user data by Claims
        /// </summary>
        /// <param name="claims">holds the user claims</param>
        /// <returns>User data</returns>
        public async Task<AppUser> FindUserByClaims(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }
    }
}