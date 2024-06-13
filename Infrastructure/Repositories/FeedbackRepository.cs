namespace Infrastructure.Repositories;

internal class FeedbackRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Feedback>(dbContext), IFeedbackRepository
{
    public IQueryable<GetFeedBacksResponse> GetFeedbacksByEmployeeId(int emplyeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == emplyeeId)
                .ProjectTo<GetFeedBacksResponse>(_mapper.ConfigurationProvider);
    }
}