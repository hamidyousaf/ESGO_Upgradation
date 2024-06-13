namespace Domain.Enums;

public enum ApplicationStatusEnum : byte
{
    Waiting = 0,
    Accepted = 1,
    Rejected = 2,
    AutoCancelled = 3,
    Completed = 4,
    TimeSheetApproved = 5,
    PaymentCompleted = 6,
    Cancelled = 7,
    TimesheetApprovalPending = 8,
    PaymentPending = 9,
    TimeSheetRejected = 10,
    WaitingForEmployeeConfirmation = 11,
    EmployeeAssigned = 12, // employer directly select the employee
    EmployerCancelledJob = 13,
    EmployerAccountDeactivated = 14
}
