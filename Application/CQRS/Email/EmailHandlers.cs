namespace Domain.CQRS.Email;

public sealed class SendEmailHandler(IConfiguration _configuration) : IRequestHandler<SendEmailCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var apiKey = _configuration["SendGridAPIKey"];
        // var client = new SendGridClient(apiKey);
        // var from = new EmailAddress("test@authdemo.com", "JWT Auth Demo");
        // var to = new EmailAddress(toEmail);
        // var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        // var response = await client.SendEmailAsync(msg);
        var FromEmail = _configuration["EmailSetting:FromEmail"] ?? string.Empty;
        var FromPassword = _configuration["EmailSetting:FromPassword"];
        var Host = _configuration["EmailSetting:Server"] ?? string.Empty;
        var Port = Convert.ToInt16(_configuration["EmailSetting:Port"]);
        //var ToEmail = _configuration["EmailSetting:ToEmail"] ??  string.Empty;
        var ToEmail = request.ToEmail;
        try
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(FromEmail);
            message.To.Add(new MailAddress(ToEmail));
            message.Subject = request.Subject;
            message.IsBodyHtml = true; //to make message body as html
            message.Body = request.Content;
            smtp.Port = Port;
            smtp.Host = Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FromEmail, FromPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail("Email didn't sent!");
        }


        return Result<bool>.Success(true, "Email sent successfully!");
    }
}
