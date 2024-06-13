namespace Application.Abstractions;

public interface IShadowShiftRepository : IGenericRepository<ShadowShift>
{
    IQueryable<GetShadowShiftsResponse> GetShadowShiftsByEmployeeId(int emplyeeId);
}
