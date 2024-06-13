namespace Infrastructure.Repositories;

public class TypeOfServiceRepository(DbContext dbContext) : GenericRepository<TypeOfService>(dbContext), ITypeOfServiceRepository {}
