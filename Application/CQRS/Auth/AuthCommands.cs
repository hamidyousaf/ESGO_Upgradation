﻿namespace Domain.CQRS.Auth;
public sealed class RegisterCommand : IRequest<Result<IEnumerable<string>>>
{
    public RegisterRequest Register { get; }
    public RegisterCommand(RegisterRequest register)
    {
        Register = register;
    }
}

public sealed class LoginCommand : IRequest<Result<LoginResponse>>
{
    public LoginRequest Login { get; }
    public LoginCommand(LoginRequest login)
    {
        Login = login;
    }
}

public sealed class ConfirmEmailCommand : IRequest<Result<IEnumerable<string>>>
{
    public string UserId { get; }
    public string Token { get; }
    public ConfirmEmailCommand(string userId, string token)
    {
        UserId = userId;
        Token = token;
    }
}

public sealed class ForgetPasswordCommand : IRequest<Result<bool>>
{
    public string Email { get; }
    public ForgetPasswordCommand(string email)
    {
        Email = email;
    }
}