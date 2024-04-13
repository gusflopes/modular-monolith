using Ardalis.Result;
using MediatR;
using RiverBooks.Users;
using RiverBooks.Users.CartEndpoints;

namespace RiverBooks.Users.UseCases;

public record ListCartItemsQuery(string EmailAddress) : IRequest<Result<List<CartItemDto>>>;
