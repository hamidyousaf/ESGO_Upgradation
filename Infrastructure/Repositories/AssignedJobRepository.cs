using Domain.Entities;

namespace Infrastructure.Repositories;

internal sealed class AssignedJobRepository(ApplicationDbContext dbContext, IMapper _mapper) 
    : GenericRepository<AssignedJob>(dbContext), IAssignedJobRepository
{

    public IQueryable<GetAssignedJobByIdResponce> GetAssignedJobById(int assignedJobId, int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId && x.Id == assignedJobId)
                    .Include(x => x.Employer)
                    .Include(x => x.Job)
                    .Include(x => x.Employee)
                        .ThenInclude(employee => employee.EmployeeType)
                .ProjectTo<GetAssignedJobByIdResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetAssignedJobEmployeeByIdForAdminResponse> GetAssignedJobById(int assignedJobId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == assignedJobId)
                    .Include(x => x.Employee)
                    .Include(x => x.Job)
                .ProjectTo<GetAssignedJobEmployeeByIdForAdminResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetAssignedJobsResponse> GetAssignedJobsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                    .Include(x => x.Employer)
                    .Include(x => x.Job)
                .Where(x => x.EmployeeId == employeeId && x.JobStatus == (byte)JobStatusEnum.Open)
                .ProjectTo<GetAssignedJobsResponse>(_mapper.ConfigurationProvider)
                .OrderBy(job => job.Date);
    }
    public IQueryable<GetAppliedJobsResponse> GetAppliedJobsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
              .Include(x => x.Employer)
              .Include(x => x.Job)
          .Where(x => x.EmployeeId == employeeId && x.JobStatus == (byte)JobStatusEnum.Applied)
            // also check job should exclude with cancel status.
           .Where(x => x.Job.Status != (byte)JobStatusEnum.Cancelled)
          .ProjectTo<GetAppliedJobsResponse>(_mapper.ConfigurationProvider)
          .OrderBy(job => job.Date);
    }
    public IQueryable<GetCompletedJobsResponse> GetCompletedJobs()
    {
        return GetAllReadOnly()
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == (byte) JobStatusEnum.Completed)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetCompletedJobsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetCompletedJobsResponse> GetCompletedJobsByEmployee(int employeeId)
    {
        return GetAllReadOnly()
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == (byte) JobStatusEnum.Completed && x.EmployeeId == employeeId)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetCompletedJobsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetConfirmedJobsForAdminResponse> GetConfirmedJobs()
    {
        return GetAllReadOnly()
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == (byte)JobStatusEnum.Confirmed)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetConfirmedJobsForAdminResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetConfirmedJobsResponse> GetConfirmedJobsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                    .Include(x => x.Employer)
                    .Include(x => x.Job)
                .Where(x => x.EmployeeId == employeeId && x.JobStatus == (byte) JobStatusEnum.Confirmed)
                // also check job should exclude with cancel status.
                .Where(x => x.Job.Status != (byte)JobStatusEnum.Cancelled)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetConfirmedJobsResponse>(_mapper.ConfigurationProvider);
    }
    public IQueryable<GetJobsByStatusResponse> GetJobsByStatus(byte status)
    {
        return GetAllReadOnly()
                .Include(x => x.Employee)
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == status)
                // also check job should exclude with cancel status.
                .Where(x => x.Job.Status != (byte) JobStatusEnum.Cancelled)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetJobsByStatusResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetUnsuccessfulJobsResponse> GetUnsuccessfulJobs()
    {
        return GetAllReadOnly()
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == (byte)JobStatusEnum.UnSuccessful)
                .OrderBy(job => job.Job.Date)
                .ProjectTo<GetUnsuccessfulJobsResponse>(_mapper.ConfigurationProvider);
    }
}