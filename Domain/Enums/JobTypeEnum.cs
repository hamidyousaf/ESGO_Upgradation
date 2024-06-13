namespace Domain.Enums;

public enum JobTypeEnum : byte
{
    [Description("Normal Job")]
    NormalJob =1,
    [Description("Urgent Job")]
    UrgentJob = 2,
    [Description("Unlisting Job")]
    UnlistingJob = 3
}
