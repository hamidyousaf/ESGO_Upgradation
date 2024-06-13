namespace Infrastructure.Repositories;

internal sealed class EmployerRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Employer>(dbContext), IEmployerRepository
{
    public IQueryable<GetAllEmployersByStatusResponse> GetAllPendingEmployers(byte status)
    {
        return GetAllReadOnly()
                .Where(x => x.AccountStatus == status)
                .ProjectTo<GetAllEmployersByStatusResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployerAllDetailsByIdResponce> GetEmployerAllDetailsById(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == employerId)
                .ProjectTo<GetEmployerAllDetailsByIdResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployerByIdResponse> GetEmployerById(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == employerId)
                .ProjectTo<GetEmployerByIdResponse>(_mapper.ConfigurationProvider);
    }
}