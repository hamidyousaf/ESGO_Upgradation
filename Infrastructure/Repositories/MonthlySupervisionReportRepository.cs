namespace Infrastructure.Repositories;

internal class MonthlySupervisionReportRepository(DbContext dbContext, IMapper _mapper) 
    : GenericRepository<MonthlySupervisionReport>(dbContext), IMonthlySupervisionReportRepository
{
    public IQueryable<GetMonthlySupervisionReportsResponse> GetMonthlySupervisionReportsByEmployeeId(int emplyeeId)
    {
         return GetAllReadOnly()
                .Where(x => x.EmployeeId == emplyeeId)
                .ProjectTo<GetMonthlySupervisionReportsResponse>(_mapper.ConfigurationProvider);
    }
}