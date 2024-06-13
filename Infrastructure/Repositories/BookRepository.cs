namespace Infrastructure.Repositories;

internal sealed class BookRepository(ApplicationDbContext dbContext) : GenericRepository<Book>(dbContext), IBookRepository {}
