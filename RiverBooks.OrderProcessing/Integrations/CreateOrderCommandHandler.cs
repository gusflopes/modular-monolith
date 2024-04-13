using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using Serilog;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  private readonly IOrderRepository _orderRepository;
  private readonly ILogger _logger;

  public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
  {
    _orderRepository = orderRepository;
    _logger = logger;
  }
  
  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(oi => new OrderItem(oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description));
    
    // fake address for now
    var shippingAddress = new Address("123 Main St", "", "Anytown", "WA", "98001", "USA");
    var billingAddress = new Address("123 Main St", "", "Anytown", "WA", "98001", "USA");

    var newOrder = Order.Factory.Create(request.UserId, shippingAddress, billingAddress, items);

    await _orderRepository.Add(newOrder);
    await _orderRepository.SaveChanges();
    
    _logger.Information("New Order Created! {OrderId}", newOrder.Id);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
