namespace Domain.Entities;

public class Job : BaseEntity<int>
{
    public int EmployerId { get; set; }
    public byte JobTypeId { get; set; }
    public byte EmployeeTypeId { get; set; }
    public int? BookingId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftId { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public decimal HourlyRate { get; set; }
    public byte BreakTime { get; set; }
    public decimal CostPershift { get; set; }
    public byte EmployeeCategoryId { get; set; }
    public string JobDescription { get; set; }
    public decimal CostPershiftPerDay { get; set; }
    public decimal JobHoursPerDay { get; set; }
    public decimal HolidayPayRate { get; set; }
    public bool IsDummy { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string CancellationReason { get; set; }
    public DateTime CancellationDate { get; set; }
    public byte Status { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
    // navigational property.
    public Employer Employer { get; set; }
    public EmployeeType EmployeeType { get; set; }
    public Booking? Booking { get; set; }
}
