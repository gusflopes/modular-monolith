using Ardalis.Result;
using MediatR;

namespace RiverBooks.OrderProcessing.Endpoints;

internal record ListOrdersForUserQuery(string EmailAddress) : IRequest<Result<List<OrderSummary>>>;
