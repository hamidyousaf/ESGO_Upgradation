namespace Domain.CQRS.Books;
public sealed class AddBookHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddBookCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book()
        {
            Title = request.Book.Title,
            Author = request.Book.Author,
            Description = request.Book.Description
        };
        await _unitOfWork.BookRepository.Add(book);
        var isCompleted = await _unitOfWork.IsCompleted();

        if (!isCompleted)
        {
            return Result<int>.Fail("Something went wrong.", 0);
        }

        return Result<int>.Success(book.Id, "Book added succesfully.");
    }
}

public sealed class DeleteBookHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteBookCommand, bool>
{
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        // Get book by id.
        //var book = await _unitOfWork
        //    .BookRepository
        //    .Get(request.BookId);

        //if (book is null)
        //{
        //    return false;
        //}

        await _unitOfWork.BookRepository.DeleteById(request.BookId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

public sealed class GetAllBooksHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllBooksQuery, List<Book>>
{
    async Task<List<Book>> IRequestHandler<GetAllBooksQuery, List<Book>>.Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.BookRepository
            .GetAll()
            .ToListAsync(cancellationToken);
        return result;
    }
}
public sealed class GetBookByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetBookByIdQuery, BookProjectTo_V1?>
{
    public async Task<BookProjectTo_V1?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork
            .BookRepository
            .GetAll()
            .ProjectTo_V1()
            .FirstOrDefaultAsync(x => x.Id == request.BookId, cancellationToken);

        return book;
    }
}

public sealed class UpdateBookHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateBookCommand, bool>
{
    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        // Get book by id.
        var book = await _unitOfWork
            .BookRepository
            .GetById(request.Book.Id);

        if (book is null)
        {
            return false;
        }

        book.Title = request.Book.Title;
        book.Description = request.Book.Description;
        book.Author = request.Book.Author;

        _unitOfWork.BookRepository.Change(book);
        return true;
    }
}