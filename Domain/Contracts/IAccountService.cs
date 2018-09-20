using System.Security.Claims;
using Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAccountService
    {
        Task<SignInResult> SignIn(string email, string password, bool rememberMe, bool lockOutPeriod = false);

        Task SignOut();

        Task<IdentityResult> Register(AppUser user, string password);

        Task<AppUser> FindUser(string email);

        Task<IdentityResult> ResetPassword(AppUser user, string code, string password);

        Task<string> GeneratePasswordResetToken(AppUser user);

        Task SetUserRegion(ClaimsPrincipal user, int? id);

        Task<AppUser> FindUserByClaims(ClaimsPrincipal user);
    }
}