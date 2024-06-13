namespace Domain.Enums;

public enum DocumentStatusEnum : byte
{
    Unknown = 0,
    VerificationUnderProcess = 1,
    Verified = 2,
    Rejected = 3
}
