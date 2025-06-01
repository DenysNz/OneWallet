using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
            _sendGridClient = new SendGridClient(_configuration["SendGrid:ApiKey"]);
        }

        public async Task SendSecurityCodeAsync(string emailTo, string code) 
        {
            var data = new { code = code };

            await SendTemplate(emailTo, _configuration["SendGrid:VerifyEmailTemplate"], data, _configuration["SendGrid:EmailTitle"]);
        } 

        private async Task SendTemplate(string emailTo, string templateId, object data, string emailTitle) 
        {
            var from = new EmailAddress(_configuration["SendGrid:EmailFrom"], emailTitle);
            var to = new EmailAddress(emailTo);
            SendGridMessage gridMessage = MailHelper.CreateSingleTemplateEmail(from, to, templateId, data);
            var result = await _sendGridClient.SendEmailAsync(gridMessage);
        }

        public async Task SendEmailAsync(string emailTo, string code, string emailSubject)
        {
            var apiKey = _configuration.GetSection("SendGrid:ApiKey").Value;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_configuration.GetSection("SendGrid:EmailFrom").Value, _configuration["SendGrid:EmailTitle"]),
                Subject = emailSubject,
                PlainTextContent = code,
                HtmlContent =  code
            };
            msg.AddTo(new EmailAddress(emailTo, emailTo));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
