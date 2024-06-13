namespace Infrastructure.Repositories;

internal sealed class EmployeeRepository(IMapper _mapper, ApplicationDbContext dbContext) : GenericRepository<Employee>(dbContext), IEmployeeRepository
{
    public IQueryable<GetAllEmployeesByStatusResponse> GetAllPendingEmployee(byte status)
    {
        return GetAllReadOnly()
            .Where(x => x.AccountStatus == status)
                .Include(x => x.EmployeeType)
            .ProjectTo<GetAllEmployeesByStatusResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetDocumentPolicyInfoResponse> GetDocumentPolicyInfoByEmployee(int employeeId)
    {
        return GetAllReadOnly()
            .Where(x => x.Id == employeeId)
            .ProjectTo<GetDocumentPolicyInfoResponse>(_mapper.ConfigurationProvider);
    }
    public IQueryable<GetEmployeeByIdResponse> GetEmployeeById(int employeeId)
    {
        return GetAllReadOnly()
            .Where(x => x.Id == employeeId)
            .ProjectTo<GetEmployeeByIdResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployeeBySecretIdResponse> GetEmployeeBySecretId(Guid employeeSecretId)
    {
        return GetAllReadOnly()
            .Where(x => x.UserId.Equals(employeeSecretId))
            .ProjectTo<GetEmployeeBySecretIdResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployeesByEmployeeTypeIdResponse> GetEmployeesByEmployeeTypeId(byte emplyeeTypeId)
    {
        return GetAllReadOnly()
            .Where(x => x.EmployeeTypeId == emplyeeTypeId)
            .ProjectTo<GetEmployeesByEmployeeTypeIdResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployeeAllDetailsByIdResponce> GetEmployeeAllDetailsById(int employeeId)
    {
        return GetAllReadOnly()
            .Where(x => x.Id == employeeId)
                .Include(x => x.EmployementType)
                .Include(x => x.EmployeeType)
            .ProjectTo<GetEmployeeAllDetailsByIdResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetDbsExpiredEmployeesResponse> GetDbsExpiredEmployees()
    {
        return GetAllReadOnly()
            .Where(x => x.DbsExpiryDate.HasValue 
                            && x.DbsExpiryDate < DateTime.UtcNow.Date.AddMonths(1) 
                            && x.AccountStatus == (byte) EmployeeAccountStatusEnum.Activated)
            .ProjectTo<GetDbsExpiredEmployeesResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetTrainingCertificateExpiredEmployeesResponse> GetEmployeesByIds(List<int> employeesIds)
    {
        return GetAllReadOnly()
            .Where(x => employeesIds.Contains(x.Id) && x.AccountStatus == (byte) EmployeeAccountStatusEnum.Activated)
            .ProjectTo<GetTrainingCertificateExpiredEmployeesResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetEmployeeShortDetailsByIdResponce> GetEmployeeShortDetailsById(int employeeId)
    {
        return GetAllReadOnly()
            .Where(x => x.Id == employeeId)
                .Include(x => x.EmployementType)
                .Include(x => x.EmployeeType)
            .ProjectTo<GetEmployeeShortDetailsByIdResponce>(_mapper.ConfigurationProvider);
    }
}
