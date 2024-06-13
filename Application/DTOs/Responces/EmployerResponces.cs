namespace Application.DTOs.Responces;

public class GetShiftsResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
public class GetShiftByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class EmployerLoginResponse
{
    public string Token { get; set; }
    public string Role { get; set; }
    public string OrganisationName { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool IsAdmin { get; set; } = false;
}

public class GetTimesheetsByStatusForEmployerResponse
{
    public int TimesheetId { get; set; }
    public int JobId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeType { get; set; }
    public DateTime TimesheetDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployeeName { get; set; }
    public TimeOnly BillableHours { get; set; }
    public byte BreakTime { get; set; }
    public TimeOnly TotalHours { get; set; }
    public string? Notes { get; set; } = string.Empty;
    public byte Status { get; set; }
    public string OrganisationName { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime JobCreatedDate { get; set; }
    public string ShiftName { get; set; }
    public int ShiftId { get; set; }
    public byte Rating { get; set; }
    public string ReviewedBy { get; set; }
    public decimal HourlyRate { get; set; } // from Job entity.
}

public class GetAPIsForJobForEmployerResponce
{
    public List<EmployeeTypeDto> EmployeeTypes { get; set; }
    public List<JobTypeDto> JobTypes { get; set; }
    public List<EmployeeCategoryDto> EmployeeCategories { get; set; }
    public List<ShiftDto> Shifts { get; set; }
    public List<byte> BreakTime { get; set; }
}

public class GetShiftsForJobResponce
{
    public int ShiftId { get; set; }
    public DateTime ShiftStartTime { get; set; }
    public DateTime ShiftEndTime { get; set; }
    public byte JobTypeId { get; set; }
    [NotMapped]
    public JobTypeEnum JT
    {
        get => (JobTypeEnum)JobTypeId;
        set => JobTypeId = (byte)value;
    }
    public string JobType { get => JT.ToString(); }
    public string EmployeeType { get; set; }
    public byte EmployeeTypeId { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftName { get; set; }
    public byte BreakTime { get; set; }
}

public class GetJobByIdForEmployerResponce
{
    public int Id { get; set; }
    public int EmployerId { get; set; }
    public byte JobTypeId { get; set; }
    public byte EmployeeTypeId { get; set; }
    public int? BookingId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftId { get; set; }
    public DateTime ShiftStartTime { get; set; }
    public DateTime ShiftEndTime { get; set; }
    public decimal HourlyRate { get; set; }
    public byte BreakTime { get; set; }
    public decimal CostPershift { get; set; }
    public byte EmployeeCategoryId { get; set; }
    public string JobDescription { get; set; }
    public bool IsDummy { get; set; }
    public int Applicants { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
}

public class GetApplicantsByJobIdResponce
{
    public int JobId { get; set; }
    public DateTime Date { get; set; }
    public string ShiftName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal HourlyRate { get; set; }
    public byte EmployeeTypeId { get; set; }
    public string EmployeeType { get; set; }
    public decimal JobHoursPerDay { get; set; }
    public List<ApplicantsGetApplicantsByJobIdResponce> Applicants { get; set; }
}
public class ApplicantsGetApplicantsByJobIdResponce
{
    public int SerialNumber { get; set; }
    public int AssignedJobId { get; set; }
    public int EmployeeId { get; set; }
    public string Employee { get; set; }
    public decimal HourWorked { get; set; }
    public byte JobStatus { get; set; }
    public bool IsSelected { get; set; }

}
public class GetEmployeeDetailsByIdResponce
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public bool IsFavourite { get; set; }
    public double Rating { get; set; }
    public string ProfileImageUrl { get; set; }
    public string DbsNumber { get; set; }
    public string DbsCertificateUrl { get; set; }
    public string DbsCertificateRejectionReason { get; set; }
    public byte DbsCertificateStatus { get; set; }
    public DateTime? DbsExpiryDate { get; set; }
    public byte EmployeeTypeId { get; set; }
    public string NMCPin { get; set; }
    public bool IsNMCPinVerified { get; set; }
    public byte DbsNumberStatus { get; set; }
    public bool IsReferenceVerified { get; set; }
    public bool IsRightToWorkVerified { get; set; }
    public bool IsAllDbsDocumentsVerified { get; set; }
    public List<TrainingDocumenForGetEmployeeDetailsByIdResponce> TrainingDocuments { get; set; }
    public List<DbsForGetEmployeeDetailsByIdResponce> DbsDocuments { get; set; }
    public List<EmployementDto> Experiences { get; set; }
    public List<GetFeedBacksResponse> Feedbacks { get; set; }
    
}
public class TrainingDocumenForGetEmployeeDetailsByIdResponce
{
    public int EmployeeDocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentUrl { get; set; }
    public DateTime ExpiryDate { get; set; }
    public byte Status { get; set; }
    public string Reason { get; set; }
}
public class DbsForGetEmployeeDetailsByIdResponce
{
    public string DocumentType { get; set; }
    public string GroupNo { get; set; }
    public string Url { get; set; }
    public int DocumentTypeId { get; set; }
    public byte DocumentNumber { get; set; }
    public byte Status { get; set; }
    public string? RejectionReason { get; set; }
}
public class GetEmployeesWorkedUnderEmployerResponce
{
    public int Id { get; set; }
    public string EmployeeType { get; set; }
    public string Name { get; set; }
}
public class GetFavouriteEmployeesResponce
{
    public int Id { get; set; }
    public string EmployeeType { get; set; }
    public string Name { get; set; }
}