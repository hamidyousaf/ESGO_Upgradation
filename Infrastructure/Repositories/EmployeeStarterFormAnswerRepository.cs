namespace Infrastructure.Repositories;

internal sealed class EmployeeStarterFormAnswerRepository(DbContext dbContext) 
    : GenericRepository<EmployeeStarterFormAnswer>(dbContext), IEmployeeStarterFormAnswerRepository {}
