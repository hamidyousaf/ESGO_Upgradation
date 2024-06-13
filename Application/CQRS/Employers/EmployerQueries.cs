using Domain.DTOs.Responces;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Application.CQRS.Employers;

public sealed class GetBookingsQuery : IRequest<Result<PaginationModel<GetBookingsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployerId { get; set; }

    public GetBookingsQuery(int employerId, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployerId = employerId;
    }
}

public sealed class GetShiftsQuery : IRequest<Result<PaginationModel<GetShiftsResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployerId { get; set; }

    public GetShiftsQuery(int employerId, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployerId = employerId;
    }
}

public sealed class GetTimesheetsByStatusForEmployerQuery : IRequest<Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int EmployerId { get; set; }
    public byte Status { get; set; }

    public GetTimesheetsByStatusForEmployerQuery(int employerId, byte status, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        EmployerId = employerId;
        Status = status;
    }
}

public sealed class GetShiftByIdQuery : IRequest<Result<GetShiftByIdResponse>>
{
    public int EmployerId { get; set; }
    public int ShiftId { get; set; }

    public GetShiftByIdQuery(int employerId, int shiftId)
    {
        EmployerId = employerId;
        ShiftId = shiftId;
    }
}

public sealed class GetNextBookingIdQuery : IRequest<Result<GetNextBookingIdResponse>> {}
public sealed class GetEmployerByIdQuery : IRequest<Result<GetEmployerByIdResponse>>
{
    public int EmployerId { get; }
    public GetEmployerByIdQuery(int employerId)
    {
        EmployerId = employerId;
    }
}
public sealed class GetTimesheetByIdForEmployerQuery : IRequest<Result<GetTimesheetByIdResponce>>
{
    public int EmployerId { get; }
    public int TimesheetId { get; }
    public GetTimesheetByIdForEmployerQuery(int employerId, int timesheetId)
    {
        EmployerId = employerId;
        TimesheetId = timesheetId;
    }
}

public sealed class GetAPIsForJobForEmployerQuery : IRequest<Result<GetAPIsForJobForEmployerResponce>>
{
    public int EmployerId { get; }
    public GetAPIsForJobForEmployerQuery(int employerId)
    {
        EmployerId = employerId;
    }
}

public sealed class GetShiftsForJobQuery : IRequest<Result<List<GetShiftsForJobResponce>>>
{
    public int EmployerId { get; }
    public GetShiftsForJobQuery(int employerId)
    {
        EmployerId = employerId;
    }
}

public sealed class GetJobByIdForEmployerQuery : IRequest<Result<GetJobByIdForEmployerResponce>>
{
    public int EmployerId { get; }
    public int JobId { get; }
    public GetJobByIdForEmployerQuery(int employerId, int jobId)
    {
        EmployerId = employerId;
        JobId = jobId;
    }
}

public sealed class RemoveEmployeeFromAssignedJobByIdQuery : IRequest<Result<bool>>
{
    public int EmployerId { get; }
    public int AssignedJobId { get; }
    public RemoveEmployeeFromAssignedJobByIdQuery(int employerId, int assignedJobId)
    {
        EmployerId = employerId;
        AssignedJobId = assignedJobId;
    }
}

public sealed class GetApplicantsByJobIdQuery : IRequest<Result<GetApplicantsByJobIdResponce>>
{
    public int EmployerId { get; }
    public int JobId { get; }
    public GetApplicantsByJobIdQuery(int employerId, int jobId)
    {
        EmployerId = employerId;
        JobId = jobId;
    }
}

public sealed class SelectEmployeeByAssignedJobIdQuery : IRequest<Result<bool>>
{
    public SelectEmployeeByAssignedJobIdRequest Request { get; }
    public SelectEmployeeByAssignedJobIdQuery(SelectEmployeeByAssignedJobIdRequest request)
    {
        Request = request;
    }
}

public sealed class GetEmployeeDetailsByIdQuery : IRequest<Result<GetEmployeeDetailsByIdResponce>>
{
    public int EmployeeId { get; }
    public int EmployerId { get; }
    public GetEmployeeDetailsByIdQuery(int employeeId, int employerId)
    {
        EmployeeId = employeeId;
        EmployerId = employerId;
    }
}

public sealed class GetEmployeesWorkedUnderEmployerQuery : IRequest<Result<List<GetEmployeesWorkedUnderEmployerResponce>>>
{
    public int EmployerId { get; }
    public GetEmployeesWorkedUnderEmployerQuery(int employerId)
    {
        EmployerId = employerId;
    }
}

public sealed class GetFavouriteEmployeesQuery : IRequest<Result<List<GetFavouriteEmployeesResponce>>>
{
    public int EmployerId { get; }
    public GetFavouriteEmployeesQuery(int employerId)
    {
        EmployerId = employerId;
    }
}