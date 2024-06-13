namespace Infrastructure.Repositories;

internal sealed class EmployementTypeRepository(IMapper _mapper, DbContext dbContext) 
    : GenericRepository<EmployementType>(dbContext), IEmployementTypeRepository
{
    public IQueryable<EmployementTypeResponse> GetEmployementTypes()
    {
        return GetAllReadOnly()
                .ProjectTo<EmployementTypeResponse>(_mapper.ConfigurationProvider);
    }
}