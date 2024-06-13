namespace Infrastructure.Repositories;

internal sealed class EmployementRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Employement>(dbContext), IEmployementRepository
{
    public IQueryable<GetEmployementsByEmployeeResponse> GetEmployementsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId)
                .ProjectTo<GetEmployementsByEmployeeResponse>(_mapper.ConfigurationProvider);
    }
}