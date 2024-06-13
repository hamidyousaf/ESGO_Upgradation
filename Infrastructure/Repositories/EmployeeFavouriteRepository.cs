namespace Infrastructure.Repositories;

internal sealed class EmployeeFavouriteRepository(ApplicationDbContext dbContext) 
    : GenericRepository<EmployeeFavourite>(dbContext), IEmployeeFavouriteRepository
{
}
