using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
public class EmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Your Name", _configuration["EmailSettings:Username"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };
        using (var client = new SmtpClient())
        {
            // Connect to the SMTP server
            await client.ConnectAsync(_configuration["EmailSettings:Host"],
                                       int.Parse(_configuration["EmailSettings:Port"]),
                                       SecureSocketOptions.StartTls);
            // Authenticate with the SMTP server
            await client.AuthenticateAsync(_configuration["EmailSettings:Username"],
                                            _configuration["EmailSettings:Password"]);
            // Send the email
            await client.SendAsync(emailMessage);

            // Disconnect from the SMTP server
            await client.DisconnectAsync(true);
        }
    }
}
