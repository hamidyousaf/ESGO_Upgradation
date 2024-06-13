using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string content);
    Task SendConfirmationEmailAsync(string toEmail, string userId);
    Task SendConfirmationEmailToAdminAsync(string name, string date);
    Task Send(string toEmail, string subject, string content, string username);
}
