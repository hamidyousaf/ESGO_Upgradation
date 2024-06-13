namespace Domain.CQRS.Admins;

public sealed class GetAllEmployeesByStatusQuery : IRequest<Result<PaginationModel<GetAllEmployeesByStatusResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public byte Status { get; set; }

    public GetAllEmployeesByStatusQuery(byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Status = status;
    }
}

public sealed class ChangeEmployeeStatusQuery : IRequest<Result<bool>>
{
    public ChangeEmployeeStatusRequest Request { get; set; }

    public ChangeEmployeeStatusQuery(ChangeEmployeeStatusRequest request)
    {
        Request = request;
    }
}

public sealed class GetEmployeeAllDetailsByIdQuery : IRequest<Result<GetEmployeeAllDetailsByIdResponce>>
{
    public int EmployeeId { get; }

    public GetEmployeeAllDetailsByIdQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetEmployeeShortDetailsByIdQuery : IRequest<Result<GetEmployeeShortDetailsByIdResponce>>
{
    public int EmployeeId { get; }

    public GetEmployeeShortDetailsByIdQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetEmployerAllDetailsByIdQuery : IRequest<Result<GetEmployerAllDetailsByIdResponce>>
{
    public int EmployerId { get; }

    public GetEmployerAllDetailsByIdQuery(int employeeId)
    {
        EmployerId = employeeId;
    }
}

public sealed class GetAllDocumentsByEmployeeQuery : IRequest<Result<ResultForGetDocumentsByEmployeeResponse>>
{
    public int EmployeeId { get; set; }

    public GetAllDocumentsByEmployeeQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}


public sealed class ChangeEmployeeDocumentStatusQuery : IRequest<Result<bool>>
{
    public ChangeEmployeeDocumentStatusRequest Request { get; set; }

    public ChangeEmployeeDocumentStatusQuery(ChangeEmployeeDocumentStatusRequest request)
    {
        Request = request;
    }
}

public sealed class GetAllEmployersByStatusQuery : IRequest<Result<PaginationModel<GetAllEmployersByStatusResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public byte Status { get; set; }

    public GetAllEmployersByStatusQuery(byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Status = status;
    }
}

public sealed class ChangeEmployerStatusQuery : IRequest<Result<bool>>
{
    public ChangeEmployerStatusRequest Request { get; set; }

    public ChangeEmployerStatusQuery(ChangeEmployerStatusRequest request)
    {
        Request = request;
    }
}

public sealed class GetAllBookingsQuery : IRequest<Result<PaginationModel<GetAllBookingsResponce>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public GetAllBookingsQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class ChangeBookingStatusQuery : IRequest<Result<bool>>
{
    public ChangeBookingStatusRequest Request { get; set; }

    public ChangeBookingStatusQuery(ChangeBookingStatusRequest request)
    {
        Request = request;
    }
}

public sealed class GetEmployeeTypesQuery : IRequest<Result<List<EmployeeTypeResponse>>> 
{
}

public sealed class GetEmployeeTypeByIdQuery : IRequest<Result<GetEmployeeTypeByIdResponse>> 
{
    public byte EmplyeeTypeId { get; set; }
    public GetEmployeeTypeByIdQuery(byte emplyeeTypeId)
    {
        EmplyeeTypeId = emplyeeTypeId;
    }
}

public sealed class GetShadowShiftsQuery : IRequest<Result<List<GetShadowShiftsResponse>>> 
{
    public int EmplyeeId { get; set; }
    public GetShadowShiftsQuery(int emplyeeId)
    {
        EmplyeeId = emplyeeId;
    }
}

public sealed class GetMonthlySupervisionReportsQuery : IRequest<Result<List<GetMonthlySupervisionReportsResponse>>> 
{
    public int EmplyeeId { get; set; }
    public GetMonthlySupervisionReportsQuery(int emplyeeId)
    {
        EmplyeeId = emplyeeId;
    }
}

public sealed class GetFeedbacksQuery : IRequest<Result<List<GetFeedBacksResponse>>> 
{
    public int EmplyeeId { get; set; }
    public GetFeedbacksQuery(int emplyeeId)
    {
        EmplyeeId = emplyeeId;
    }
}

public sealed class GetDbsExpiredEmployeesQuery : IRequest<Result<PaginationModel<GetDbsExpiredEmployeesResponse>>> 
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public GetDbsExpiredEmployeesQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetTrainingCertificateExpiredEmployeesQuery : IRequest<Result<PaginationModel<GetTrainingCertificateExpiredEmployeesResponse>>> 
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public GetTrainingCertificateExpiredEmployeesQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetEmployeesByEmployeeTypeIdQuery : IRequest<Result<List<GetEmployeesByEmployeeTypeIdResponse>>> 
{
    public byte EmplyeeTypeId { get; set; }
    public GetEmployeesByEmployeeTypeIdQuery(byte emplyeeTypeId)
    {
        EmplyeeTypeId = emplyeeTypeId;
    }
}

public sealed class GetShiftsByEmployerIdQuery : IRequest<Result<List<GetShiftsResponse>>> 
{
    public int EmployerId { get; set; }
    public GetShiftsByEmployerIdQuery(int employerId)
    {
        EmployerId = employerId;
    }
}

public sealed class GetJobsByStatusQuery : IRequest<Result<PaginationModel<GetJobsByStatusResponse>>> 
{
    public byte Status { get; }
    public int PageIndex { get; }
    public int PageSize { get; } 
    public int EmployerId { get; } 

    public GetJobsByStatusQuery(byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Status = status;
    }

}

public sealed class GetJobsByStatusForEmployerQuery : IRequest<Result<PaginationModel<GetJobsByStatusResponse>>> 
{
    public byte Status { get; }
    public int PageIndex { get; }
    public int PageSize { get; } 
    public int EmployerId { get; }

    public GetJobsByStatusForEmployerQuery(int employerId, byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Status = status;
        EmployerId = employerId;
    }
}

public sealed class GetUnsuccessfulJobsQuery : IRequest<Result<PaginationModel<GetUnsuccessfulJobsResponse>>> 
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;

    public GetUnsuccessfulJobsQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetConfirmedJobsForAdminQuery : IRequest<Result<PaginationModel<GetConfirmedJobsForAdminResponse>>> 
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;

    public GetConfirmedJobsForAdminQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetCompletedJobsQuery : IRequest<Result<PaginationModel<GetCompletedJobsResponse>>> 
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;

    public GetCompletedJobsQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetAssignedJobEmployeeByIdForAdminQuery : IRequest<Result<GetAssignedJobEmployeeByIdForAdminResponse>> 
{
    public int AssignedJobId { get; }

    public GetAssignedJobEmployeeByIdForAdminQuery(int assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}

public sealed class GetEmployersForInvoiceQuery : IRequest<Result<List<GetEmployersForInvoiceResponse>>> { }

public sealed class GetTotalAmountOfEmployerQuery : IRequest<Result<GetTotalAmountOfEmployerResponse>>
{
    public GetTotalAmountOfEmployerRequest Request { get; }

    public GetTotalAmountOfEmployerQuery(GetTotalAmountOfEmployerRequest request)
    {
        Request = request;
    }
}

public sealed class GetNextInvoiceNumberQuery : IRequest<Result<GetNextInvoiceNumberResponse>> { }

public sealed class GetEmployeeProfileAsPdfQuery : IRequest<Result<string>>
{
    public int EmployeeId { get; }

    public GetEmployeeProfileAsPdfQuery(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class GetAPIsForJobQuery : IRequest<Result<GetAPIsForJobResponce>>
{
    public int BookingId { get; }
    public GetAPIsForJobQuery(int bookingId)
    {
        BookingId = bookingId;
    }
}

public sealed class GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdQuery : IRequest<Result<List<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce>>>
{
    public byte EmployeeTypeId { get; }
    public byte EmployeeCategoryId { get; }
    public GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdQuery(byte employeeTypeId, byte employeeCategoryId)
    {
        EmployeeTypeId = employeeTypeId;
        EmployeeCategoryId = employeeCategoryId;
    }
}

public sealed class GetDashboardDetailsQuery : IRequest<Result<GetDashboardDetailsResponce>> {}

public sealed class GetTimesheetsByStatusForAdminQuery : IRequest<Result<PaginationModel<GetTimesheetsByStatusForAdminResponce>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public byte Status { get; set; }

    public GetTimesheetsByStatusForAdminQuery(byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Status = status;
    }
}
public sealed class GetTimesheetByIdForAdminQuery : IRequest<Result<GetTimesheetByIdForAdminResponce>>
{
    public int TimesheetId { get; }
    public GetTimesheetByIdForAdminQuery(int timesheetId)
    {
        TimesheetId = timesheetId;
    }
}
public sealed class GetAssignedJobByIdForAdminQuery : IRequest<Result<GetAssignedJobByIdForAdminResponce>>
{
    public int AssignedJobId { get; }
    public GetAssignedJobByIdForAdminQuery(int assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}
public sealed class GetEmployeeByAssignedJobIdQuery : IRequest<Result<GetEmployeeByAssignedJobIdResponce>>
{
    public int AssignedJobId { get; }
    public GetEmployeeByAssignedJobIdQuery(int assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}
public sealed class GetEmployeeJobDetailsByIdQuery : IRequest<Result<GetEmployeeJobDetailsByIdResponce>>
{
    public int AssignedJobId { get; }
    public GetEmployeeJobDetailsByIdQuery(int assignedJobId)
    {
        AssignedJobId = assignedJobId;
    }
}
public sealed class GetEmployerJobsQuery : IRequest<Result<PaginationModel<GetEmployerJobsResponce>>>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int EmployerId { get; }
    public string TabName { get; }
    public GetEmployerJobsQuery(int employerId,string tabName, int pageIndex, int pageSize)
    {
        EmployerId = employerId;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TabName = tabName;
    }
}
public sealed class GetJobHistoryByEmployeeIdQuery : IRequest<Result<PaginationModel<GetJobHistoryByEmployeeIdResponce>>>
{
    public int EmployeeId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public GetJobHistoryByEmployeeIdQuery(int employeeId, int pageIndex, int pageSize)
    {
        EmployeeId = employeeId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
public sealed class GetEmployeesByJobIdQuery : IRequest<Result<List<GetEmployeesByJobIdResponce>>>
{
    public int JobId { get; }
    public GetEmployeesByJobIdQuery(int jobId)
    {
        JobId = jobId;
    }
}
public sealed class GetNotificationsQuery : IRequest<Result<GetNotificationsResponce>> 
{
    public string Type { get; }
    public int Skip { get; }

    public GetNotificationsQuery(string type, int skip = 0)
    {
        Type = type;
        Skip = skip;
    }
}