namespace Application.Abstractions;

public interface IDbsDocumentRepository : IGenericRepository<DbsDocument>
{
    IQueryable<DbsDocumentDto> GetDbsDocumentsByEmployeeId(int employeeId);
}
