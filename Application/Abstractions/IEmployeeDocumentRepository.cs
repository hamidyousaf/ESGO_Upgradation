namespace Application.Abstractions;

public interface IEmployeeDocumentRepository : IGenericRepository<EmployeeDocument>
{
    IQueryable<GetDocumentsByEmployeeResponse> GetDocumentsByEmployee(int employeeId);
}
