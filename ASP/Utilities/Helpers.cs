using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Utilities;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ASP.Utilities
{
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
                message = await emailService.SendExceptionEmail(ex.GetType().Name, ex.Message, ex.ToString()) ? message : Constants.Message.NullException;
            }
            else if (ex.InnerException is ArgumentOutOfRangeException)
            {
                message = await emailService.SendExceptionEmail(ex.GetType().Name, ex.Message, ex.ToString()) ? message : Constants.Message.OutOfRangeException;
            }
            else
            {
                await emailService.SendExceptionEmail(ex.GetType().Name, ex.Message, ex.ToString());
            }

            return message;
        }

        public static string CreateValidationSummary(ModelStateDictionary modelState)
        {
            var message = string.Empty;
            if (modelState.IsValid)
                return string.Empty;
            
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