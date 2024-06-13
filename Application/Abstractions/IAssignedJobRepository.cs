namespace Application.Abstractions;

public interface IAssignedJobRepository : IGenericRepository<AssignedJob>
{
    IQueryable<GetAssignedJobsResponse> GetAssignedJobsByEmployeeId(int employeeId);
    IQueryable<GetAppliedJobsResponse> GetAppliedJobsByEmployeeId(int employeeId);
    IQueryable<GetConfirmedJobsResponse> GetConfirmedJobsByEmployeeId(int employeeId);
    IQueryable<GetAssignedJobByIdResponce> GetAssignedJobById(int assignedJobId, int employeeId);
    IQueryable<GetAssignedJobEmployeeByIdForAdminResponse> GetAssignedJobById(int assignedJobId);
    IQueryable<GetJobsByStatusResponse> GetJobsByStatus(byte status);
    IQueryable<GetUnsuccessfulJobsResponse> GetUnsuccessfulJobs();
    IQueryable<GetConfirmedJobsForAdminResponse> GetConfirmedJobs();
    IQueryable<GetCompletedJobsResponse> GetCompletedJobs();
    IQueryable<GetCompletedJobsResponse> GetCompletedJobsByEmployee(int employeeId);
}
