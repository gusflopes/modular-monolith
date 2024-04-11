using FastEndpoints;

namespace RiverBooks.Books;

internal class UpdateBookPriceEndpoint(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Post("/api/books/{Id}/pricehistory");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
  {
    // TODO: Handle not Found
    await _bookService.UpdateBookPrice(req.Id, req.NewPrice);
    var updatedBook = await _bookService.GetBookById(req.Id);
    await SendAsync(updatedBook);
  }
}
