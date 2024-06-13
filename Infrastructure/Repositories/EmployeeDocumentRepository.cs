namespace Infrastructure.Repositories;

internal sealed class EmployeeDocumentRepository(DbContext dbContext, IMapper _mapper) 
    : GenericRepository<EmployeeDocument>(dbContext), IEmployeeDocumentRepository
{
    public IQueryable<GetDocumentsByEmployeeResponse> GetDocumentsByEmployee(int employeeId)
    {
        return GetAllReadOnly()
                    .Include(document => document.Document)
                .Where(x => x.EmployeeId == employeeId)
                .ProjectTo<GetDocumentsByEmployeeResponse>(_mapper.ConfigurationProvider);
    }
}