namespace Application.CQRS.BackgroundJobs;


public sealed class ConvertJobStatusFromConfirmToCompleteCommand : IRequest<Result<bool>>
{
    public ConvertJobStatusFromConfirmToCompleteCommand() {}
}

public sealed class ConvertJobStatusFromOpenToUnsuccessfulCommand : IRequest<Result<bool>>
{
    public ConvertJobStatusFromOpenToUnsuccessfulCommand() {}
}