using Data.Models.Entities;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendExceptionEmail(string exceptionName, string stackTrace);

        Task<bool> SendForgotPasswordEmail(AppUser user, string url);
    }
}