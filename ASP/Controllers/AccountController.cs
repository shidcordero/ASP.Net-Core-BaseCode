using ASP.Extensions;
using Data.Models.Entities;
using Data.ViewModels.Account;
using Data.ViewModels.Common;
using Domain.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ASP.Utilities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ASP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignIn(model.Email, model.Password, model.RememberMe);
                    if (result == SignInResult.Success)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new AppUser { UserName = model.Email, Email = model.Email };
                    var result = await _accountService.Register(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToLocal(returnUrl);
                    }

                    ModelState.AddIdentityErrors(result);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid register attempt.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindUser(model.Email);
                    if (user == null)
                    {
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    var code = await _accountService.GeneratePasswordResetToken(user);
                    var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                    if (await _emailService.SendForgotPasswordEmail(user, callbackUrl))
                    {
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid forgot password attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindUser(model.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist
                        return RedirectToAction(nameof(ResetPasswordConfirmation));
                    }

                    var result = await _accountService.ResetPassword(user, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ResetPasswordConfirmation));
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid reset password attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion Helpers
    }
}