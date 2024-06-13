namespace Application.Abstractions;

public interface IShiftRepository : IGenericRepository<Shift>
{
    IQueryable<GetShiftsResponse> GetShiftsByEmployerId(int employerId);
    IQueryable<GetShiftByIdResponse> GetShiftForEmployerById(int shiftId, int employerId);
}
