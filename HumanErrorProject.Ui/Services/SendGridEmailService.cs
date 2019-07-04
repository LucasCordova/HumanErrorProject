using System.Threading.Tasks;
using HumanErrorProject.Engine;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Ui.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HumanErrorProject.Ui.Services
{
    public class SendGridEmailService : IEmailService
    {
        public SendGridOptions Options;

        public SendGridEmailService(IOptions<SendGridOptions> options)
        {
            Options = options.Value;
        }

        public async Task Send(EmailData data)
        {
            var client = new SendGridClient(Options.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.Email, Options.Name),
                Subject = data.Subject,
                PlainTextContent = data.Content,
            };
            msg.AddTo(new EmailAddress(data.Email, data.Name));
            await client.SendEmailAsync(msg);
        }
    }
}
