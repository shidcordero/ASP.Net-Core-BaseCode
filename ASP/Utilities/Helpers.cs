using System;
using System.Threading.Tasks;
using Data.Utilities;
using Domain.Contracts;

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
    }
}