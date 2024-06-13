namespace Domain.Enums;

public enum JobStatusEnum : byte
{
    Open = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3,
    UnSuccessful = 4,
    Applied = 6,
    RemovedByEmployer = 7
}