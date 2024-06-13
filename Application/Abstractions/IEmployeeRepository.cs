namespace Domain.Abstractions;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    IQueryable<GetAllEmployeesByStatusResponse> GetAllPendingEmployee(byte status);
    IQueryable<GetEmployeeBySecretIdResponse> GetEmployeeBySecretId(Guid employeeSecretId);
    IQueryable<GetDocumentPolicyInfoResponse> GetDocumentPolicyInfoByEmployee(int employeeId);
    IQueryable<GetEmployeesByEmployeeTypeIdResponse> GetEmployeesByEmployeeTypeId(byte emplyeeTypeId);
    IQueryable<GetEmployeeByIdResponse> GetEmployeeById(int employeeId);
    IQueryable<GetEmployeeAllDetailsByIdResponce> GetEmployeeAllDetailsById(int employeeId);
    IQueryable<GetEmployeeShortDetailsByIdResponce> GetEmployeeShortDetailsById(int employeeId);
    IQueryable<GetDbsExpiredEmployeesResponse> GetDbsExpiredEmployees();
    IQueryable<GetTrainingCertificateExpiredEmployeesResponse> GetEmployeesByIds(List<int> employeesIds);
}
