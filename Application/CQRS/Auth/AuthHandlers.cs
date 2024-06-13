namespace Domain.CQRS.Auth;

public sealed class ConfirmEmailCommandHandlers(UserManager<User> _userManger) : IRequestHandler<ConfirmEmailCommand, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // userId and token is null or empty.
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Token))
        {
            return Result<IEnumerable<string>>.Fail("Invalid userid or token.");
        }

        // check user exists.
        var user = await _userManger.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Result<IEnumerable<string>>.Fail("User not found.");
        }

        // decode the token.
        var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
        string normalToken = Encoding.UTF8.GetString(decodedToken);

        // confirm the email.
        var result = await _userManger.ConfirmEmailAsync(user, normalToken);

        if (result.Succeeded)
        {
            return Result<IEnumerable<string>>.Success(new List<string>(), "Email confirmed successfully!");
        }

        return Result<IEnumerable<string>>.Fail("Email did not confirm.", result.Errors.Select(e => e.Description));
    }
}

public sealed class ForgetPasswordCommandHandlers(UserManager<User> _userManger, IConfiguration _configuration, IMediator _mediator) 
    : IRequestHandler<ForgetPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        // check email is correct.
        if (string.IsNullOrEmpty(request.Email))
        {
            return Result<bool>.Fail("Invalid email.");
        }

        // check eser exists.
        var user = await _userManger.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<bool>.Fail("No user associated with email.");
        }

        // generate token to send on email.
        var token = await _userManger.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        string url = $"{_configuration["AppUrl"]}/ResetPassword?email={request.Email}&token={validToken}";

        // send email.
        string content = $"<h1>Follow the instructions to reset your password</h1>" +
            $"<p>To reset your password <a href='{url}'>Click here</a></p>";

        await _mediator.Send(new SendEmailCommand(request.Email, "Reset Password", content));

        return Result<bool>.Success(true, "Reset password URL has been sent to the email successfully!");
    }
}
//public sealed class ResetPasswordCommandHandlers(UserManager<User> _userManger) : IRequestHandler<ResetPasswordCommand, Result<IEnumerable<string>>>
//{
//    public async Task<Result<IEnumerable<string>>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
//    {
//        // check eser exists.
//        var user = await _userManger.FindByEmailAsync(request.ResetPassword.Email);
//        if (user is null)
//        {
//            return Result<IEnumerable<string>>.Fail("No user associated with email.");
//        }

//        // Compare the Password and ConfirmPassword fields.
//        if (request.ResetPassword.NewPassword != request.ResetPassword.ConfirmPassword)
//        {
//            return Result<IEnumerable<string>>.Fail("Password doesn't match its confirmation.");
//        }

//        // decode the token.
//        var decodedToken = WebEncoders.Base64UrlDecode(request.ResetPassword.Token);
//        string normalToken = Encoding.UTF8.GetString(decodedToken);

//        // reset the password.
//        var result = await _userManger.ResetPasswordAsync(user, normalToken, request.ResetPassword.NewPassword);
//        if (result.Succeeded)
//        {
//            return Result<IEnumerable<string>>.Success(new List<string>(), "Password has been reset successfully!");
//        }

//        return Result<IEnumerable<string>>.Fail("Something went wrong", result.Errors.Select(e => e.Description));
//    }
//}