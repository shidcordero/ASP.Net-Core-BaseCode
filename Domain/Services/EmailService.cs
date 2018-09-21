using Data.Configurations;
using Data.Contracts;
using Data.Models.Entities;
using Data.Utilities;
using Domain.Contracts;
using Domain.ServiceModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using EmailTemplate = Data.Utilities.Constants.EmailTemplate;

namespace Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ExceptionEmailConfiguration _exceptionEmailConfig;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailService(IOptions<EmailConfiguration> emailConfig,
            IOptions<ExceptionEmailConfiguration> exceptionEmailConfig,
            IEmailTemplateRepository emailTemplateRepository)
        {
            _emailConfig = emailConfig.Value;
            _exceptionEmailConfig = exceptionEmailConfig.Value;
            _emailTemplateRepository = emailTemplateRepository;
        }

        /// <summary>
        /// Sends email request
        /// </summary>
        /// <param name="mailRequest">mail request data</param>
        private Task SendMailRequest(MailRequest mailRequest)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_emailConfig.FromDisplayName, _emailConfig.FromAddress.Trim()));
            // To
            if (mailRequest.ToList == null || mailRequest.ToList.Length == 0)
            {
                mimeMessage.To.Add(new MailboxAddress(mailRequest.To.Trim()));
            }
            else
            {
                foreach (var to in mailRequest.ToList)
                {
                    mimeMessage.To.Add(new MailboxAddress(to.Trim()));
                }
            }
            // CC
            if (!string.IsNullOrEmpty(mailRequest.Cc))
            {
                mimeMessage.Cc.Add(new MailboxAddress(mailRequest.Cc.Trim()));
            }
            else
            {
                if (mailRequest.CcList != null && mailRequest.CcList.Length > 0)
                {
                    foreach (var cc in mailRequest.CcList)
                    {
                        mimeMessage.Cc.Add(new MailboxAddress(cc.Trim()));
                    }
                }
            }
            // BCC
            if (!string.IsNullOrEmpty(mailRequest.Bcc))
            {
                mimeMessage.Bcc.Add(new MailboxAddress(mailRequest.Bcc.Trim()));
            }
            else
            {
                if (mailRequest.BccList != null && mailRequest.BccList.Length > 0)
                {
                    foreach (var bcc in mailRequest.BccList)
                    {
                        mimeMessage.Bcc.Add(new MailboxAddress(bcc.Trim()));
                    }
                }
            }

            mimeMessage.Subject = mailRequest.Subject;
            mimeMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = mailRequest.Body
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                // client.Connect(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
                client.Connect(_emailConfig.Host, _emailConfig.Port, false);
                client.AuthenticationMechanisms.Remove(Constants.Common.Xoauth2);
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mimeMessage);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Used to send email notification for exception emails
        /// </summary>
        /// <param name="exceptionName">holds the exception name</param>
        /// <param name="stackTrace">holds the stacktrace</param>
        /// <returns>Boolean Data</returns>
        public async Task<bool> SendExceptionEmail(string exceptionName, string stackTrace)
        {
            var template = await _emailTemplateRepository.FindByTemplateName(_exceptionEmailConfig.TemplateName);
            if (template == null) return false;

            var mailRequest = new MailRequest
            {
                To = _exceptionEmailConfig.To,
                Subject = string.Format(template.Subject, exceptionName),
                Body = string.Format(template.Body, stackTrace)
            };
            await SendMailRequest(mailRequest);

            return true;
        }

        /// <summary>
        /// USed to send email notification for forgot password
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="url">holds the callback url</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> SendForgotPasswordEmail(AppUser user, string url)
        {
            var template = await _emailTemplateRepository.FindByTemplateName(EmailTemplate.ForgotPassword.TemplateName);
            if (template == null) return false;

            var mailRequest = new MailRequest
            {
                To = _exceptionEmailConfig.To,
                Subject = template.Subject,
                Body = string.Format(template.Body, $"{user.FirstName} {user.LastName}", url)
            };
            await SendMailRequest(mailRequest);

            return true;
        }
    }
}