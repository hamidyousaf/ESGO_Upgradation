namespace Domain.CQRS.Employees;

public sealed class SignUpAPIsQuery : IRequest<Result<SignUpAPIsResponse>>
{
}

public sealed class AddVaccinationQuery : IRequest<Result<bool>>
{
    public AddVaccinationRequest Request { get; }
    public AddVaccinationQuery(AddVaccinationRequest request)
    {
        Request = request;
    }
}
public sealed class AddShiftsQuery : IRequest<Result<bool>>
{
    public AddShiftsRequest Request { get; }
    public AddShiftsQuery(AddShiftsRequest request)
    {
        Request = request;
    }
}

public sealed class GetQualificationsByEmployeeQuery : IRequest<Result<List<GetQualificationsByEmployeeResponse>>>
{
    public int EmployeeId { get; }
    public GetQualificationsByEmployeeQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class AddHaveQualificationQuery : IRequest<Result<bool>>
{
    public AddHaveQualificationRequest Request { get; }
    public AddHaveQualificationQuery(AddHaveQualificationRequest request )
    {
        Request = request;
    }
}

public sealed class AddIsSubjectOfInvestigationQuery : IRequest<Result<bool>>
{
    public AddIsSubjectOfInvestigationRequest Request { get; }
    public AddIsSubjectOfInvestigationQuery(AddIsSubjectOfInvestigationRequest request )
    {
        Request = request;
    }
}

public sealed class GetEmployementsByEmployeeQuery : IRequest<Result<List<GetEmployementsByEmployeeResponse>>>
{
    public int EmployeeId { get; }
    public GetEmployementsByEmployeeQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetEmployeeForPersonalBySecretIdQuery : IRequest<Result<GetEmployeeBySecretIdResponse>>
{
    public Guid EmployeeSecretId { get; }
    public GetEmployeeForPersonalBySecretIdQuery(Guid employeeSecretId)
    {
        EmployeeSecretId = employeeSecretId;
    }
}

public sealed class GetEmployeeForProfessionalBySecretIdQuery : IRequest<Result<GetEmployeeBySecretIdResponse>>
{
    public Guid EmployeeSecretId { get; }
    public GetEmployeeForProfessionalBySecretIdQuery(Guid employeeSecretId)
    {
        EmployeeSecretId = employeeSecretId;
    }
}

public sealed class CanAddPersonalReferenceQuery : IRequest<Result<CanAddPersonalReferenceResponse>>
{
    public Guid EmployeeSecretId { get; }
    public CanAddPersonalReferenceQuery(Guid employeeSecretId)
    {
        EmployeeSecretId = employeeSecretId;
    }
}

public sealed class CanAddProfessionalReferenceQuery : IRequest<Result<CanAddProfessionalReferenceResponse>>
{
    public Guid EmployeeSecretId { get; }
    public CanAddProfessionalReferenceQuery(Guid employeeSecretId)
    {
        EmployeeSecretId = employeeSecretId;
    }
}

public sealed class GetEmployeeDocumentsQuery : IRequest<Result<List<GetEmployeeDocumentsResponse>>>
{
    public int EmployeeId { get; }
    public GetEmployeeDocumentsQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}


public sealed class GetDocumentPolicyInfoQuery : IRequest<Result<GetDocumentPolicyInfoResponse>>
{
    public int EmployeeId { get; }
    public GetDocumentPolicyInfoQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetEmployeeByIdQuery : IRequest<Result<GetEmployeeByIdResponse>>
{
    public int EmployeeId { get; }
    public GetEmployeeByIdQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetStarterFormAnswersQuery : IRequest<Result<List<GetStarterFormAnswerResponse>>>
{
    public int EmployeeId { get; }
    public GetStarterFormAnswersQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetAssignedJobsQuery : IRequest<Result<PaginationModel<GetAssignedJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetAssignedJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetUrgentJobsQuery : IRequest<Result<PaginationModel<GetUrgentJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetUrgentJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetAllJobsQuery : IRequest<Result<PaginationModel<GetAllJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetAllJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetFavouriteJobsQuery : IRequest<Result<PaginationModel<GetFavouriteJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetFavouriteJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetAppliedJobsQuery : IRequest<Result<PaginationModel<GetAppliedJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetAppliedJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetConfirmedJobsQuery : IRequest<Result<PaginationModel<GetConfirmedJobsResponse>>>
{

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetConfirmedJobsQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetCompletedJobsByEmployeeQuery : IRequest<Result<PaginationModel<GetCompletedJobsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public byte Radius { get; set; }
    public string PostalCode { get; set; }
    public GetCompletedJobsByEmployeeQuery(int employeeId, int pageIndex, int pageSize, DateTime fromDate, decimal minRate, byte radius, string postalCode)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        FromDate = fromDate;
        MinRate = minRate;
        Radius = radius;
        PostalCode = postalCode;
    }
}

public sealed class GetTimesheetsByStatusQuery : IRequest<Result<PaginationModel<GetTimesheetsByStatusResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployeeId { get; set; }
    public byte Status { get; set; }

    public GetTimesheetsByStatusQuery(int employeeId, byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployeeId = employeeId;
        Status = status;
    }
}

public sealed class ChangeJobStatusToConfirmQuery : IRequest<Result<bool>>
{
    public int EmployeeId { get; set; }
    public int AssignedJobId { get; set; }
    public ChangeJobStatusToConfirmQuery(int employeeId, int assignedJobId)
    {
        EmployeeId = employeeId;
        AssignedJobId = assignedJobId;
    }
}

public sealed class GetAssignedJobByIdQuery : IRequest<Result<GetAssignedJobByIdResponce>>
{
    public int EmployeeId { get; set; }
    public int AssignedJobId { get; set; }
    public GetAssignedJobByIdQuery(int employeeId, int assignedJobId)
    {
        EmployeeId = employeeId;
        AssignedJobId = assignedJobId;
    }
}

public sealed class GetJobByIdQuery : IRequest<Result<GetJobByIdResponce>>
{
    public int EmployeeId { get; set; }
    public int JobId { get; set; }
    public GetJobByIdQuery(int employeeId, int jobId)
    {
        EmployeeId = employeeId;
        JobId = jobId;
    }
}

public sealed class GetJobByIdForHomePageQuery : IRequest<Result<GetJobByIdResponce>>
{
    public int JobId { get; set; }
    public GetJobByIdForHomePageQuery(int jobId)
    {
        JobId = jobId;
    }
}

public sealed class GetTimesheetByIdQuery : IRequest<Result<GetTimesheetByIdResponce>>
{
    public int EmployeeId { get; set; }
    public int TimesheetId { get; set; }
    public GetTimesheetByIdQuery(int employeeId, int timesheetId)
    {
        EmployeeId = employeeId;
        TimesheetId = timesheetId;
    }
}

public sealed class GetPendingJobCountsQuery : IRequest<Result<GetPendingJobCountsResponce>>
{
    public int EmployeeId { get; set; }
    public GetPendingJobCountsQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetTimesheetCountsQuery : IRequest<Result<GetTimesheetCountsResponce>>
{
    public int EmployeeId { get; set; }
    public GetTimesheetCountsQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetJobCountsQuery : IRequest<Result<GetJobCountsResponce>>
{
    public int EmployeeId { get; set; }
    public GetJobCountsQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class ApplyJobQuery : IRequest<Result<bool>>
{
    public int EmployeeId { get; set; }
    public ApplyJobRequest Request { get; set; }
    public ApplyJobQuery(int employeeId, ApplyJobRequest request)
    {
        EmployeeId = employeeId;
        Request = request;
    }
}

public sealed class GetEmployeeAllDetailsByIdForEmployeeQuery : IRequest<Result<GetEmployeeAllDetailsByIdForEmployeeResponce>>
{
    public int EmployeeId { get; set; }
    public GetEmployeeAllDetailsByIdForEmployeeQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class HomeJobSearchQuery : IRequest<Result<PaginationModel<HomeJobSearchResponce>>>
{
    public HomeJobSearchRequest Request { get; set; }
    public HomeJobSearchQuery(HomeJobSearchRequest request)
    {
        Request = request;
    }
}

public sealed class PendingJobSearchQuery : IRequest<Result<PaginationModel<PendingJobSearchResponce>>>
{
    public PendingJobSearchRequest Request { get; set; }
    public PendingJobSearchQuery(PendingJobSearchRequest request)
    {
        Request = request;
    }
}

public sealed class CompletedJobSearchQuery : IRequest<Result<PaginationModel<CompletedJobSearchResponce>>>
{
    public CompletedJobSearchRequest Request { get; set; }
    public CompletedJobSearchQuery(CompletedJobSearchRequest request)
    {
        Request = request;
    }
}

public sealed class GetOpenJobsForHomePageQuery : IRequest<Result<PaginationModel<GetOpenJobsForHomePageResponce>>>
{
    public GetOpenJobsForHomePageRequest Request { get; set; }
    public GetOpenJobsForHomePageQuery(GetOpenJobsForHomePageRequest request)
    {
        Request = request;
    }
}

public sealed class WebGuestJobSearchQuery : IRequest<Result<PaginationModel<WebGuestJobSearchResponce>>>
{
    public WebGuestJobSearchRequest Request { get; set; }
    public WebGuestJobSearchQuery(WebGuestJobSearchRequest request)
    {
        Request = request;
    }
}

public sealed class GetCalenderJobsQuery : IRequest<Result<GetCalenderJobsResponce>>
{
    public GetCalenderJobsRequest Request { get; set; }
    public GetCalenderJobsQuery(GetCalenderJobsRequest request)
    {
        Request = request;
    }
}

public sealed class GetDocumentTypesQuery : IRequest<Result<GetDocumentTypesResponce>>
{
    public GetDocumentTypesQuery() {}
}

public sealed class EmailVerificationQuery : IRequest<Result<bool>>
{
    public string Token { get; set; }
    public EmailVerificationQuery(string token)
    {
        Token = token;
    }
}

public sealed class GetAssignedJobByEncryptIdQuery : IRequest<Result<GetAssignedJobByEncryptIdResponce>>
{
    public string AssignedJobId { get; set; }
    public GetAssignedJobByEncryptIdQuery(string assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}

public sealed class ChangeJobStatusToConfirmWithEmailQuery : IRequest<Result<bool>>
{
    public string AssignedJobId { get; set; }
    public ChangeJobStatusToConfirmWithEmailQuery(string assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}