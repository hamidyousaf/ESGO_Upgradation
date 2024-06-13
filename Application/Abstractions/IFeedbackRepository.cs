namespace Application.Abstractions;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    IQueryable<GetFeedBacksResponse> GetFeedbacksByEmployeeId(int emplyeeId);
}
