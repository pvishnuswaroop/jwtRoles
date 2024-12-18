using MailKit.Net.Smtp;
using MimeKit;

namespace QuickServeAPP.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("QuickServe", smtpSettings["SenderEmail"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpSettings["Server"], int.Parse(smtpSettings["Port"]), false);
            await smtp.AuthenticateAsync(smtpSettings["SenderEmail"], smtpSettings["SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
