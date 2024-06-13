namespace Infrastructure.Repositories;

internal sealed class DbsDocumentRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<DbsDocument>(dbContext), IDbsDocumentRepository
{
    public IQueryable<DbsDocumentDto> GetDbsDocumentsByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                    .Include(document => document.DocumentType)
                .Where(document => document.EmployeeId == employeeId)
                .ProjectTo<DbsDocumentDto>(_mapper.ConfigurationProvider);
    }
}