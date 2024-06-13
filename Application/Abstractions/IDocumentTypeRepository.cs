namespace Application.Abstractions;

public interface IDocumentTypeRepository : IGenericRepository<DocumentType>
{
    IQueryable<DocumentTypeForGetDocumentTypesResponce> GetDocumentTypes();
}
