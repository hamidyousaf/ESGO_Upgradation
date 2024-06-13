namespace Domain.Abstractions;

public interface IEmployeeTypeRepository : IGenericRepository<EmployeeType>
{
    IQueryable<EmployeeTypeResponse> GetEmployeeTypes(); 
    IQueryable<GetEmployeeTypeByIdResponse> GetEmployeeTypeById(byte emplyeeTypeId); 
}
