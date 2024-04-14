using Ardalis.Result;
using MediatR;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrdersForUserQueryHandler : IRequestHandler<ListOrdersForUserQuery, Result<List<OrderSummary>>>
{
  private readonly IOrderRepository _orderRepository;

  public ListOrdersForUserQueryHandler(IOrderRepository orderRepository)
  {
    _orderRepository = orderRepository;
  }

  public async Task<Result<List<OrderSummary>>> Handle(ListOrdersForUserQuery request, CancellationToken cancellationToken)
  {
    // look up UserId for EmailAddress
    
    // TODO: Filter By User
    var orders = await _orderRepository.List();
    var summaries = orders.Select(o => new OrderSummary(
        o.Id,
        o.UserId,
        o.DateCreated,
        DateTimeOffset.Now,
        o.OrderItems.Sum(oi => oi.UnitPrice)) // need to include OrderItems
    ).ToList();

    return summaries;
  }
}
