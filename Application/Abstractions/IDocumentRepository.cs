namespace Application.Abstractions;

public interface IDocumentRepository : IGenericRepository<Document>
{
    IQueryable<GetDocumentByCategoryIdResponse> GetDocumentByCategoryId(byte categoryId);
}
