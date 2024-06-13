namespace Application.Abstractions;

public interface IJobRepository : IGenericRepository<Job>
{
    IQueryable<GetJobByIdResponce> GetJobById(int jobId);
}
