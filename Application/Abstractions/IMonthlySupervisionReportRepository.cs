namespace Application.Abstractions;

public interface IMonthlySupervisionReportRepository : IGenericRepository<MonthlySupervisionReport>
{
    IQueryable<GetMonthlySupervisionReportsResponse> GetMonthlySupervisionReportsByEmployeeId(int emplyeeId);
}
