namespace Infrastructure.Repositories;

internal sealed class StarterFormQuestionRepository(DbContext dbContext) 
    : GenericRepository<StarterFormQuestion>(dbContext), IStarterFormQuestionRepository {}
