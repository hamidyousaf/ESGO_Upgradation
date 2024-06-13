namespace Application.Abstractions;

public interface IBookingRepository : IGenericRepository<Booking>
{
    IQueryable<GetBookingsResponse> GetBookings(int employerId);
    IQueryable<GetAllBookingsResponce> GetAllBookings();
    IQueryable<GetAPIsForJobResponce> GetDetailsForJobById(int bookingId);
}
