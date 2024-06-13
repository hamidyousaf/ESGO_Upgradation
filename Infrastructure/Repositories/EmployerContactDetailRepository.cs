namespace Infrastructure.Repositories;

internal sealed class EmployerContactDetailRepository(DbContext dbContext) 
    : GenericRepository<EmployerContactDetail>(dbContext), IEmployerContactDetailRepository {}
