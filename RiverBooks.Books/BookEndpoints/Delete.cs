using FastEndpoints;

namespace RiverBooks.Books;

public record DeleteBookRequest(Guid Id);

internal class Delete(IBookService bookService) : Endpoint<DeleteBookRequest>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Delete("/books/{Id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(DeleteBookRequest req, CancellationToken ct)
  {
    await _bookService.DeleteBook(req.Id);
    await SendNoContentAsync();
  }
}
