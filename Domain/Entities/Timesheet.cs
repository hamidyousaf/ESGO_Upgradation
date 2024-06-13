namespace Domain.Entities;

public class Timesheet : BaseEntity<int>
{
    public int AssignedJobId { get; set; }
    public int JobId { get; set; } // from Job entity.
    public int EmployerId { get; set; } // from Job entity.
    public int EmployeeId { get; set; } // from AssignedJob entity.
    public DateTime Date { get; set; } // from Job entity.
    public TimeOnly StartTime { get; set; } // from Job entity.
    public TimeOnly EndTime { get; set; } // from Job entity.
    public byte BreakTime { get; set; } // from Job entity.
    public TimeOnly BillableHours { get; set; } // Formula: (EndTime - StartTime) - BreakTime;
    public decimal BillableHourInDecimal { get; set; } // For example: BillableHours = "08:30" then BillableHourInDecimal = 8.50
    public TimeOnly TotalHours { get; set; } // Formula: (EndTime - StartTime);
    public decimal HourlyRate { get; set; } // from Job entity.
    public decimal TotalAmount { get; set; } // Formula: BillableHourInDecimal * HourlyRate;
    public byte Status { get; set; } // Enum: TimeSheetStatus
    public decimal OriginalHourlyRate { get; set; } // from Job entity : Cost
    public decimal OrginalTotalAmount { get; set; } // Formula: BillableHourInDecimal * OriginalHourlyRate;
    public decimal TotalHolidayAmount { get; set; }  // Formula: HolidayPayRate from Job entity * BillableHourInDecimal;
    public string? Notes { get; set; }
    public string? Reason{ get; set; }
    public decimal? PlateformFee { get; set; }
    public decimal? FinalAmount { get; set; }
    public string ReviewedBy { get; set; }
    public byte Rating { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? RejectionDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    // navigational property.
    public AssignedJob AssignedJob { get; set; }
    public Job Job { get; set; }
    public Employer Employer { get; set; }
    public Employee Employee { get; set; }
}