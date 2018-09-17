using Data.Utilities;
using Data.ViewModels.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace ASP.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Adds a model errors for each validation result from the business service.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="validationResults">The validation results from a business service.</param>
        /// <param name="defaultErrorKey">The default key to use if a field is not specified in a business service validation result.</param>
        public static void AddControllerErrors(this Controller controller, IEnumerable<ValidationResult> validationResults, string defaultErrorKey = null)
        {
            if (validationResults == null) return;

            foreach (var validationResult in validationResults)
            {
                if (!string.IsNullOrEmpty(validationResult.MemberName))
                {
                    controller.ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
                }
                else if (defaultErrorKey != null)
                {
                    controller.ModelState.AddModelError(defaultErrorKey, validationResult.Message);
                }
                else
                {
                    controller.ModelState.AddModelError(string.Empty, validationResult.Message);
                }
            }
        }

        /// <summary>
        /// Adds a model errors for each validation result from the business service.
        /// </summary>
        /// <param name="validationResults">The validation results from a business service.</param>
        /// <param name="modelState">The model state dictionary used to add errors.</param>
        /// <param name="defaultErrorKey">The default key to use if a field is not specified in a business service validation result.</param>
        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<ValidationResult> validationResults, string defaultErrorKey = null)
        {
            if (validationResults == null) return;

            foreach (var validationResult in validationResults)
            {
                modelState.AddModelError(Constants.Common.ModelStateErrors, validationResult.Message);
            }
        }

        /// <summary>
        /// Adds a model error for each validation result from the business service.
        /// </summary>
        /// <param name="validationResult">The validation result from a business service.</param>
        /// <param name="modelState">The model state dictionary used to add errors.</param>
        public static void AddModelError(this ModelStateDictionary modelState, ValidationResult validationResult)
        {
            if (validationResult == null) return;

            modelState.AddModelError(Constants.Common.ModelStateErrors, validationResult.Message);
        }

        /// <summary>
        /// Adds a model error for each Identity result from the business service.
        /// </summary>
        /// <param name="modelState">The model state dictionary used to add errors.</param>
        /// <param name="result">Identity Result instance</param>
        public static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(Constants.Common.IdentityErrors, error.Description);
            }
        }
    }
}