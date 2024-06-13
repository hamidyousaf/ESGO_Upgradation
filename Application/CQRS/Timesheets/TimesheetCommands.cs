namespace Application.CQRS.Timesheets;

public sealed class ChangeTimeSheetVerificationCommand : IRequest<Result<bool>>
{
    public int TimesheetId { get; }
    public byte Status { get; }
    public ChangeTimeSheetVerificationCommand(int timesheetId, byte status)
    {
        TimesheetId = timesheetId;
        Status = status;
    }
}