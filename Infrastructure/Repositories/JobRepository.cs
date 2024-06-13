namespace Infrastructure.Repositories;

internal sealed class JobRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Job>(dbContext), IJobRepository
{
    public IQueryable<GetJobByIdResponce> GetJobById(int jobId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == jobId)
                    .Include(x => x.EmployeeType)
                    .Include(x => x.Employer)
                .ProjectTo<GetJobByIdResponce>(_mapper.ConfigurationProvider);
    }
}