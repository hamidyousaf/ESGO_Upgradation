namespace Application.CQRS.Employers;

public sealed class EmployerRegisterCommand : IRequest<Result<IEnumerable<string>>>
{
    public EmployerRegisterRequest Request { get; }
    public EmployerRegisterCommand(EmployerRegisterRequest request)
    {
        Request = request;
    }
}

public sealed class EmployerLoginCommand : IRequest<Result<EmployerLoginResponse>>
{
    public EmployerLoginRequest Request { get; }
    public EmployerLoginCommand(EmployerLoginRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateEmployerCommand : IRequest<Result<bool>>
{
    public UpdateEmployerRequest Request { get; }
    public UpdateEmployerCommand(UpdateEmployerRequest request)
    {
        Request = request;
    }
}

public sealed class AddEmployerCommand : IRequest<Result<int>>
{
    public EmployerRegisterRequest Request { get; }
    public AddEmployerCommand(EmployerRegisterRequest request)
    {
        Request = request;
    }
}

public sealed class AddBookingCommand : IRequest<Result<bool>>
{
    public AddBookingRequest Request { get; }
    public AddBookingCommand(AddBookingRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateBookingCommand : IRequest<Result<bool>>
{
    public UpdateBookingRequest Request { get; }
    public UpdateBookingCommand(UpdateBookingRequest request)
    {
        Request = request;
    }
}

public sealed class AddShiftCommand : IRequest<Result<bool>>
{
    public AddShiftRequest Request { get; }
    public AddShiftCommand(AddShiftRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateShiftCommand : IRequest<Result<bool>>
{
    public UpdateShiftRequest Request { get; }
    public UpdateShiftCommand(UpdateShiftRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteShiftCommand : IRequest<Result<bool>>
{
    public DeleteShiftRequest Request { get; }
    public DeleteShiftCommand(DeleteShiftRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteBookingCommand : IRequest<Result<bool>>
{
    public DeleteBookingRequest Request { get; }
    public DeleteBookingCommand(DeleteBookingRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteJobCommand : IRequest<Result<bool>>
{
    public DeleteJobRequest Request { get; }
    public DeleteJobCommand(DeleteJobRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeTimesheetStatusCommand : IRequest<Result<bool>>
{
    public ChangeTimesheetStatusRequest Request { get; }
    public ChangeTimesheetStatusCommand(ChangeTimesheetStatusRequest request)
    {
        Request = request;
    }
}

public sealed class AddOrganisationImageCommand : IRequest<Result<bool>>
{
    public AddOrganisationImageRequest Request { get; }
    public AddOrganisationImageCommand(AddOrganisationImageRequest request)
    {
        Request = request;
    }
}

public sealed class AddOrganisationLogoCommand : IRequest<Result<bool>>
{
    public AddOrganisationLogoRequest Request { get; }
    public AddOrganisationLogoCommand(AddOrganisationLogoRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateTimesheatCommand : IRequest<Result<bool>>
{
    public int EmployerId { get; }
    public UpdateTimeSheetRequest Request { get; set; }
    public UpdateTimesheatCommand(
        int employerId,
        UpdateTimeSheetRequest request)
    {
        Request = request;
        EmployerId = employerId;
    }
}

public sealed class UpdateJobCommand : IRequest<Result<bool>>
{
    public UpdateJobRequest Request { get; }
    public UpdateJobCommand(UpdateJobRequest request)
    {
        Request = request;
    }
}

public sealed class AddToFavouriteForEmployerCommand : IRequest<Result<bool>>
{
    public AddToFavouriteForEmployerRequest Request { get; }
    public AddToFavouriteForEmployerCommand(AddToFavouriteForEmployerRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteFeedbackCommand : IRequest<Result<bool>>
{
    public int FeedbackId { get; }
    public DeleteFeedbackCommand(int feedbackId)
    {
        FeedbackId = feedbackId;
    }
}

public sealed class UpdateFeedbackCommand : IRequest<Result<bool>>
{
    public UpdateFeedbackRequest Request { get; }
    public UpdateFeedbackCommand(UpdateFeedbackRequest request)
    {
        Request = request;
    }
}
