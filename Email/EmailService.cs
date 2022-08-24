using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace Email
{
    public class EmailService : IEmailService
    {
        private IConfiguration _email;
        public EmailService(IConfiguration configuration)
        {
            _email = configuration.GetSection("EmailConfig");
        }

        public void SendMail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("userhubservice@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("userhubservice@gmail.com", "Testas123");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}