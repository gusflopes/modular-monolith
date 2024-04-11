﻿using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace RiverBooks.Books;

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Get("/api/books");
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
