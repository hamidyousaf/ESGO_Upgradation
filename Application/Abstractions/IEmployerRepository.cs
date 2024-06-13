namespace Application.Abstractions;

public interface IEmployerRepository : IGenericRepository<Employer>
{
    IQueryable<GetAllEmployersByStatusResponse> GetAllPendingEmployers(byte status);
    IQueryable<GetEmployerAllDetailsByIdResponce> GetEmployerAllDetailsById(int employerId);
    IQueryable<GetEmployerByIdResponse> GetEmployerById(int employerId);
}
