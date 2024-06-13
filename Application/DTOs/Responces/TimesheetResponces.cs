namespace Application.DTOs.Responces;

public class GetTimesheetsResponse
{
    public int JobId { get; set; }
    public int EmployerId { get; set; }
    public DateTime? WorkDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public byte? BreakTime { get; set; }
    public DateTime? BillableHours { get; set; }
    public decimal? TotalHours { get; set; }
    public decimal? HoursRate { get; set; }
    public decimal? TotalAmount { get; set; }
    public byte Status { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public decimal? PlateformFess { get; set; }
    public decimal? FinalAmount { get; set; }
    public decimal? BillableHourInDecimal { get; set; }
    public decimal? OrginalTotalAmount { get; set; }
    public decimal? OriginalHourlyRate { get; set; }
    public decimal? TotalHolidayAmount { get; set; }
}
