namespace Infrastructure.Repositories;

internal class ShadowShiftRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<ShadowShift>(dbContext), IShadowShiftRepository
{
    public IQueryable<GetShadowShiftsResponse> GetShadowShiftsByEmployeeId(int emplyeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == emplyeeId)
                .ProjectTo<GetShadowShiftsResponse>(_mapper.ConfigurationProvider);
    }
}