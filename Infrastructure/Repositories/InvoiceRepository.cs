namespace Infrastructure.Repositories;

internal sealed class InvoiceRepository(ApplicationDbContext dbContext) : GenericRepository<Invoice>(dbContext), IInvoiceRepository {}
