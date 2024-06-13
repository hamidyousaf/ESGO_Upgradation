namespace Domain.Abstractions;

public interface IEmployementRepository : IGenericRepository<Employement>
{
    IQueryable<GetEmployementsByEmployeeResponse> GetEmployementsByEmployeeId(int employeeId);
}
