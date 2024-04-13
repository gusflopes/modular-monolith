using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace RiverBooks.Books;

public class ListBooksResponse
{
  public List<BookDto> Books { get; set; } = new();
}

internal class List(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        var books = await _bookService.ListBooks();

        await SendAsync(new ListBooksResponse()
        {
            Books = books
        });
    }
}
