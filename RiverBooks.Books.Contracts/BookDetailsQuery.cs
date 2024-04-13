using Ardalis.Result;
using MediatR;

namespace RiverBooks.Books.Contracts;

public record GetBookDetailsQuery(Guid BookId) : IRequest<Result<BookDetailsResponse>>;

