using FastEndpoints;

namespace RiverBooks.Books;

internal class CreateBookEndpoint(IBookService bookService) : Endpoint<CreateBookRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Post("/api/books");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
  {
    var newBookDto = new BookDto(req.Id ?? Guid.NewGuid(), req.Title, req.Author, req.Price);
    await _bookService.CreateBook(newBookDto);
    await SendCreatedAtAsync<GetBookByIdEndpoint>(new {newBookDto.Id}, newBookDto);
  }
}
