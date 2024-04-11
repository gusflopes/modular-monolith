using FastEndpoints;

namespace RiverBooks.Books;

internal class DeleteBookEndpoint(IBookService bookService) : Endpoint<DeleteBookRequest>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Delete("/api/books/{Id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(DeleteBookRequest req, CancellationToken ct)
  {
    await _bookService.DeleteBook(req.Id);
    await SendNoContentAsync();
  }
}
