namespace Domain.Abstractions;

public interface IQualificationRepository : IGenericRepository<Qualification>
{
    IQueryable<GetQualificationsByEmployeeResponse> GetQualificationsByEmployeeId(int employeeId);
}
