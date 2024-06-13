//using MimeKit;
using Application.Abstractions.Services;
using Domain.Common;
using Domain.Entities;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendConfirmationEmailToAdminAsync(string name, string date)
    {
        var content = "";

        using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Template\\EmailTemplate.html")))
        {
            content = reader.ReadToEnd();
            content = content.Replace("{UserName}", "");
            content = content.Replace("{NotificationContent}", $"{name} has registered on {date}");
        }
        var toEmail = _configuration["EmailSetting:ToEmail"] ?? string.Empty;

        await SendEmailAsync(toEmail, "New employee registered", content);
    }

    public async Task Send(string toEmail, string subject, string content, string username)
    {
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Template\\EmailTemplate.html")))
        {
            body = reader.ReadToEnd();
            body = body.Replace("{UserName}", username);
            body = body.Replace("{NotificationContent}", content);
        }

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string userId)
    {
        var content = "";

        using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Template\\MailTemplate.html")))
        {
            content = reader.ReadToEnd();
            string url = $"{_configuration["WebsiteSetting:Url"]}/mailconfirmation/" + userId;
            content = content.Replace("{Url}", url);
        }

        await SendEmailAsync(toEmail, "ESGO: Account Confirmation", content);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        var FromEmail = _configuration["EmailSetting:FromEmail"] ?? string.Empty;
        var FromPassword = _configuration["EmailSetting:FromPassword"];
        var Host = _configuration["EmailSetting:Server"] ?? string.Empty;
        var Port = Convert.ToInt16(_configuration["EmailSetting:Port"]);
        var ToEmail = toEmail;
        using (MailMessage message = new MailMessage())
        {
            message.From = new MailAddress(FromEmail);
            message.To.Add(new MailAddress(ToEmail));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html
            message.Body = content;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Port = Port;
                smtp.Host = Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromEmail, FromPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 10000; // 10 seconds (adjust as needed)

                smtp.Send(message);
            }
        }
    }
}