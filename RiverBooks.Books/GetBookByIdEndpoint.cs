using FastEndpoints;

namespace RiverBooks.Books;

internal class GetBookByIdEndpoint(IBookService bookService) : Endpoint<GetBookByIdRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Get("/api/books/{Id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
  {
    var book = await _bookService.GetBookById(req.Id);
    if (book is null)
    {
      await SendNotFoundAsync();
      return;
    }
    await SendAsync(book);
  }
}
