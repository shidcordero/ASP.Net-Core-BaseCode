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
using Data.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Helpers = ASP.Utilities.Helpers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ASP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRegionService _regionService;
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IRegionService regionService, IEmailService emailService)
        {
            _accountService = accountService;
            _regionService = regionService;
            _emailService = emailService;
        }

        /// <summary>
        /// Loads the login view
        /// </summary>
        /// <param name="returnUrl">If user is unauthenticated, this will hold the url to be redirected after login</param>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Set the return url
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Post request for User Login
        /// </summary>
        /// <param name="loginViewModel">Login user data</param>
        /// <param name="returnUrl">The url the user will be redirected after login</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            // Set the return url
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignIn(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe);
                    if (result == SignInResult.Success)
                    {
                        // If successful login, redirect to return url
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password entered.");
            return View(loginViewModel);
        }

        /// <summary>
        /// Loads the Registration View
        /// </summary>
        /// <param name="returnUrl">If user is unauthenticated, this will hold the url to be redirected after login</param>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            // Set the return url
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// Post request for Registration
        /// </summary>
        /// <param name="registerViewModel">Register user data</param>
        /// <param name="returnUrl">The url the user will be redirected after login</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            // Set the return url
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new AppUser
                    {
                        UserName = registerViewModel.Email,
                        Email = registerViewModel.Email,
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName
                    };

                    var result = await _accountService.Register(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        // if successful, this will automatically logged in the user.
                        // Redirect to the return url
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
            return View(registerViewModel);
        }

        /// <summary>
        /// Post request for Logout
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Loads the Forgot Password View
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Post request for Forgot Password
        /// </summary>
        /// <param name="forgotPassViewModel">Forgot Password user data</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if user exists
                    var user = await _accountService.FindUserByEmail(forgotPassViewModel.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist. Redirect to success screen
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    // Generate callback url for forgot password email
                    var code = await _accountService.GeneratePasswordResetToken(user);
                    var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                    if (await _emailService.SendForgotPasswordEmail(user, callbackUrl))
                    {
                        // if success in sending email, redirect to success screen
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
            return View(forgotPassViewModel);
        }

        /// <summary>
        /// Loads the Forgot Password Confirmation View
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Loads the Reset Password View
        /// </summary>
        /// <param name="code">the code generated from forgot password</param>
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

        /// <summary>
        /// Post request for Reset Password
        /// </summary>
        /// <param name="resetPassViewModel">Reset Password user data</param>s
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindUserByEmail(resetPassViewModel.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist
                        return RedirectToAction(nameof(ResetPasswordConfirmation));
                    }

                    var result = await _accountService.ResetPassword(user, resetPassViewModel.Code, resetPassViewModel.Password);
                    if (result.Succeeded)
                    {
                        // if success, redirect to reset password confirmation screen
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
            return View(resetPassViewModel);
        }

        /// <summary>
        /// Loads the Reset Password Confirmation View
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Loads the Access Denied View
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Loads the User Region View
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UserRegion()
        {
            if (User == null)
            {
                ModelState.AddModelError(string.Empty, Constants.Message.ErrorProcessing);

                TempData[Constants.Common.ModalTitle] = Constants.Message.Error;
                TempData[Constants.Common.ModalMessage] = Helpers.CreateValidationSummary(ModelState);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var ddl = await _regionService.GetRegionDropdownItem();
            var user = await _accountService.FindUserByClaims(User);
            // creates region dropdown with pre-selected value by RegionId set from user
            ViewBag.RegionDropdown = new SelectList(ddl, Constants.Common.Value, Constants.Common.Text, user.RegionId);

            return View();
        }

        /// <summary>
        /// Post request for User Region
        /// </summary>
        /// <param name="id">Holds the RegionId</param>
        [HttpPost]
        public async Task<IActionResult> UserRegion(int? id)
        {
            try
            {
                if (User != null)
                {
                    await _accountService.SetUserRegion(User, id);

                    TempData[Constants.Common.ModalMessage] = Constants.Message.RecordSuccessUpdate;
                    return RedirectToAction(nameof(UserRegion));
                }

                ModelState.AddModelError(string.Empty, "User not found!");
                // logout if user is not found, redirect to Home screen
                await _accountService.SignOut();
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid user update.");

            TempData[Constants.Common.ModalTitle] = Constants.Message.Error;
            TempData[Constants.Common.ModalMessage] = Helpers.CreateValidationSummary(ModelState);

            return RedirectToAction(nameof(UserRegion));
        }

        #region Helpers
        /// <summary>
        /// Helper function for Redirecting to return url
        /// </summary>
        /// <param name="returnUrl">If user is unauthenticated, this will hold the url to be redirected after login</param>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion Helpers
    }
}