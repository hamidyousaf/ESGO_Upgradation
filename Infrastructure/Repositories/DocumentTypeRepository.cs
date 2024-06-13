namespace Infrastructure.Repositories;

internal sealed class DocumentTypeRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<DocumentType>(dbContext), IDocumentTypeRepository
{
    public IQueryable<DocumentTypeForGetDocumentTypesResponce> GetDocumentTypes()
    {
        return GetAllReadOnly()
                .ProjectTo<DocumentTypeForGetDocumentTypesResponce>(_mapper.ConfigurationProvider);
    }
}
