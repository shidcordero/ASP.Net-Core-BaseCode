using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Utilities;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP.Utilities
{
    /// <summary>
    /// Helpers for ASP Solution only
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Used to get errors from exception
        /// </summary>
        /// <param name="ex">Holds the exceptionn data</param>
        /// <param name="emailService"></param>
        /// <returns>Error Messages</returns>
        public static async Task<string> GetErrors(Exception ex, IEmailService emailService)
        {
            var message = Constants.Message.ErrorProcessing;
            if (ex == null) return string.Empty;

            if (ex.InnerException is ArgumentNullException)
            {
                message = await emailService.SendExceptionEmail(ex.GetType().Name, ex.ToString()) ? message : Constants.Message.NullException;
            }
            else if (ex.InnerException is ArgumentOutOfRangeException)
            {
                message = await emailService.SendExceptionEmail(ex.GetType().Name, ex.ToString()) ? message : Constants.Message.OutOfRangeException;
            }
            else
            {
                await emailService.SendExceptionEmail(ex.GetType().Name, ex.ToString());
            }

            return message;
        }

        /// <summary>
        /// Used to create Validation Summary from model state
        /// </summary>
        /// <param name="modelState">Holds the validation information</param>
        /// <returns>HTML formatted error</returns>
        public static string CreateValidationSummary(ModelStateDictionary modelState)
        {
            var message = string.Empty;
            // recheck if modelstate is valid
            if (modelState.IsValid)
                return string.Empty;
            
            // generates HTML from the list of validation errors added in model state
            message += "<div class='text-danger validation-summary-errors' data-valmsg-summary='true'><ul>";
            message = modelState
                .Reverse()
                .Select(x => x.Key).SelectMany(key => modelState[key].Errors)
                .Aggregate(message, (current, err) => current + ("<li>" + err.ErrorMessage + "</li>"));
            message += "</ul></div>";

            return message;
        }
    }
}