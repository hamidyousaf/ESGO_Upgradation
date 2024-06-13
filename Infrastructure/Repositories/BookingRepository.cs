namespace Infrastructure.Repositories;

internal sealed class BookingRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Booking>(dbContext), IBookingRepository
{
    public IQueryable<GetAllBookingsResponce> GetAllBookings()
    {
        return GetAllReadOnly()
                .Include(booking => booking.Employer)
                .ProjectTo<GetAllBookingsResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetBookingsResponse> GetBookings(int employerId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployerId == employerId)
                .ProjectTo<GetBookingsResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetAPIsForJobResponce> GetDetailsForJobById(int bookingId)
    {
        return GetAllReadOnly()
                    .Include(booking => booking.Employer)
                .Where(booking => booking.Id == bookingId && booking.Status == (byte) BookingStatusEnum.Inprogress)
                .ProjectTo<GetAPIsForJobResponce>(_mapper.ConfigurationProvider);
    }
}