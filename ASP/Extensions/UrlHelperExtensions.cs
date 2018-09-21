using ASP.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates Callback URL for ResetPassword
        /// </summary>
        /// <param name="urlHelper">URLHelper instance to be extended</param>
        /// <param name="userId">Holds the user id</param>
        /// <param name="code">Holds the code generated from Forgot Password</param>
        /// <param name="scheme">Http request scheme</param>
        /// <returns>Reset password callback url</returns>
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}