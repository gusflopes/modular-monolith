using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Books.Integrations;

internal class BookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, Result<BookDetailsResponse>>
{
  private readonly IBookService _bookService;

  public BookDetailsQueryHandler(IBookService bookService)
  {
    _bookService = bookService;
  }

  public async Task<Result<BookDetailsResponse>> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
  {
    var book = await _bookService.GetBookById(request.BookId);

    if (book is null)
    {
      return Result.NotFound();
    }

    var response = new BookDetailsResponse(book.Id, book.Title, book.Author, book.Price);

    return response;
  }
}
