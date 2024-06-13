namespace Infrastructure.Repositories;

internal sealed class ShiftRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Shift>(dbContext), IShiftRepository
{
    public IQueryable<GetShiftByIdResponse> GetShiftForEmployerById(int shiftId, int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == shiftId && x.EmployerId == employerId)
                .ProjectTo<GetShiftByIdResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetShiftsResponse> GetShiftsByEmployerId(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployerId == employerId)
                .ProjectTo<GetShiftsResponse>(_mapper.ConfigurationProvider);
    }
}
