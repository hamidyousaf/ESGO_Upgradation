namespace Domain.Abstractions;

public interface IEmployementTypeRepository : IGenericRepository<EmployementType>
{
    IQueryable<EmployementTypeResponse> GetEmployementTypes();
}
