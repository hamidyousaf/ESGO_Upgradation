namespace Domain.CQRS.Books;

public sealed class AddBookCommand : IRequest<Result<int>>
{
    public AddBookRequest Book { get; }
    public AddBookCommand(AddBookRequest book)
    {
        Book = book;
    }
}

public sealed class DeleteBookCommand : IRequest<bool>
{
    public int BookId { get; }
    public DeleteBookCommand(int bookId)
    {
        BookId = bookId;
    }
}
public sealed class UpdateBookCommand : IRequest<bool>
{
    public UpdateBookRequest Book { get; }
    public UpdateBookCommand(UpdateBookRequest book)
    {
        Book = book;
    }
}
