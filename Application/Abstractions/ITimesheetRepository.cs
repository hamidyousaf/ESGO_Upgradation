namespace Application.Abstractions;

public interface ITimesheetRepository : IGenericRepository<Timesheet>
{
    IQueryable<GetTimesheetsByStatusResponse> GetTimesheetsByEmployee(int employeeId);
    IQueryable<GetTimesheetsByStatusForEmployerResponse> GetTimesheetsByEmployer(int employerId);
    IQueryable<GetTimesheetByIdResponce> GetTimesheetById(int timesheetId, int employeeId);
    IQueryable<GetTimesheetsResponse> GetTimesheets(int employerId);
    IQueryable<GetTimesheetsResponse> GetTimesheetByJobId(int jobId);
    IQueryable<GetTimesheetsResponse> GetApprovedTimeSheetsByJobId(int jobId);
    IQueryable<GetTimesheetsResponse> GetRejectedTimeSheetsByJobId(int jobId);
    IQueryable<GetTimesheetsResponse> GetPendingTimeSheetsByJobId(int jobId);
    IQueryable<GetTimesheetsByStatusForAdminResponce> GetTimesheetsByStatus(byte status);
    IQueryable<GetTimesheetsResponse> GetEmployeeListByPaymentStatus(GetEmployeeListByPaymentStatusRequest request);
}
