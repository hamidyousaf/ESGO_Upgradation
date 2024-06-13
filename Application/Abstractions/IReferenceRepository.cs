namespace Application.Abstractions;

public interface IReferenceRepository : IGenericRepository<Reference>
{
    IQueryable<GetPersonalReferenceByEmployeeIdResponce> GetPersonalReferenceByEmployeeId(int employeeId);
    IQueryable<GetProfessionalReferenceByEmployeeIdResponce> GetProfessionalReferenceByEmployeeId(int employeeId);
}
