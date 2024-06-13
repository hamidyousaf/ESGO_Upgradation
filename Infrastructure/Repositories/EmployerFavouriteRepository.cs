namespace Infrastructure.Repositories;

internal sealed class EmployerFavouriteRepository(ApplicationDbContext dbContext)
    : GenericRepository<EmployerFavourite>(dbContext), IEmployerFavouriteRepository
{
}
