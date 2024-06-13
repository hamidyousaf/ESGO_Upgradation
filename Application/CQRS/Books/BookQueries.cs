namespace Domain.CQRS.Books;

public sealed class GetAllBooksQuery : IRequest<List<Book>>
{
}

public sealed class GetBookByIdQuery : IRequest<BookProjectTo_V1>
{
    public int BookId { get; }
    public GetBookByIdQuery(int bookId)
    {
        BookId = bookId;
    }
}
