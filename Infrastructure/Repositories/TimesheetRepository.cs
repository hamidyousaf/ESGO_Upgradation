namespace Infrastructure.Repositories;

internal sealed class TimesheetRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Timesheet>(dbContext), ITimesheetRepository
{
    public IQueryable<GetTimesheetsResponse> GetTimesheets(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployerId == employerId)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsResponse> GetTimesheetByJobId(int jobId)
    {
        return GetAllReadOnly()
                .Where(x => x.JobId == jobId)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsResponse> GetApprovedTimeSheetsByJobId(int jobId)
    {
        return GetAllReadOnly()
                .Where(x => x.JobId == jobId && x.Status == (byte) TimeSheetStatusEnum.Approved)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsResponse> GetRejectedTimeSheetsByJobId(int jobId)
    {
        return GetAllReadOnly()
                .Where(x => x.JobId == jobId && x.Status == (byte)TimeSheetStatusEnum.Rejected)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsResponse> GetPendingTimeSheetsByJobId(int jobId)
    {
        return GetAllReadOnly()
                .Where(x => x.JobId == jobId && x.Status == (byte)TimeSheetStatusEnum.Pending)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsResponse> GetEmployeeListByPaymentStatus(GetEmployeeListByPaymentStatusRequest request)
    {
        return GetAllReadOnly()
                .Where(x => x.Status == request.Status)
                .ProjectTo<GetTimesheetsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsByStatusResponse> GetTimesheetsByEmployee(int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId)
                    .Include(x => x.Employer)
                    .Include(x => x.Job)
                .ProjectTo<GetTimesheetsByStatusResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetByIdResponce> GetTimesheetById(int timesheetId, int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId)
                    .Include(x => x.Employer)
                .ProjectTo<GetTimesheetByIdResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsByStatusForEmployerResponse> GetTimesheetsByEmployer(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployerId == employerId)
                    .Include(x => x.Employer)
                    .Include(x => x.Employee)
                        .ThenInclude(x => x.EmployeeType)
                    .Include(x => x.Job)
                .ProjectTo<GetTimesheetsByStatusForEmployerResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTimesheetsByStatusForAdminResponce> GetTimesheetsByStatus(byte status)
    {
        return GetAllReadOnly()
                .Include(x => x.Employer)
                .Include(x => x.Employee)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                .ProjectTo<GetTimesheetsByStatusForAdminResponce>(_mapper.ConfigurationProvider);
    }
}
