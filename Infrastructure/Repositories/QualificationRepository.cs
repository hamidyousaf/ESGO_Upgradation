namespace Infrastructure.Repositories;

internal sealed class QualificationRepository(DbContext dbContext, IMapper _mapper) 
    : GenericRepository<Qualification>(dbContext), IQualificationRepository
{
    public IQueryable<GetQualificationsByEmployeeResponse> GetQualificationsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId)
                .ProjectTo<GetQualificationsByEmployeeResponse>(_mapper.ConfigurationProvider);
    }
}