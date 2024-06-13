namespace Application.CQRS.Timesheets;

public sealed class GetTimesheetsQuery : IRequest<Result<PaginationModel<GetTimesheetsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployerId { get; set; }

    public GetTimesheetsQuery(int employerId, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployerId = employerId;
    }
}

public sealed class GetTimesheetByJobIdQuery : IRequest<Result<GetTimesheetsResponse>>
{
    public int JobId { get; }
    public int EmployerId { get; }

    public GetTimesheetByJobIdQuery(int jobId, int employerId)
    {
        JobId = jobId;
        EmployerId = employerId;
    }
}

public sealed class GetApprovedTimeSheetsByJobIdQuery : IRequest<Result<PaginationModel<GetTimesheetsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int JobId { get; }
    public int EmployerId { get; }

    public GetApprovedTimeSheetsByJobIdQuery(int jobId, int employerId, int pageIndex, int pageSize)
    {
        JobId = jobId;
        EmployerId = employerId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetRejectedTimeSheetByJobIdQuery : IRequest<Result<PaginationModel<GetTimesheetsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int JobId { get; }
    public int EmployerId { get; }

    public GetRejectedTimeSheetByJobIdQuery(int jobId, int employerId, int pageIndex, int pageSize)
    {
        JobId = jobId;
        EmployerId = employerId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetPendingTimeSheetByJobIdQuery : IRequest<Result<PaginationModel<GetTimesheetsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int JobId { get; }
    public int EmployerId { get; }

    public GetPendingTimeSheetByJobIdQuery(int jobId, int employerId, int pageIndex, int pageSize)
    {
        JobId = jobId;
        EmployerId = employerId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

public sealed class GetEmployeeListByPaymentStatusQuery : IRequest<Result<PaginationModel<GetTimesheetsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public GetEmployeeListByPaymentStatusRequest Request { get; set; }

    public GetEmployeeListByPaymentStatusQuery(GetEmployeeListByPaymentStatusRequest request,int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Request = request;
    }
}