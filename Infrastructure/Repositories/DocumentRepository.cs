namespace Infrastructure.Repositories;

internal sealed class DocumentRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Document>(dbContext), IDocumentRepository
{
    public IQueryable<GetDocumentByCategoryIdResponse> GetDocumentByCategoryId(byte categoryId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeTypeId == categoryId)
                .ProjectTo<GetDocumentByCategoryIdResponse>(_mapper.ConfigurationProvider);
    }
}