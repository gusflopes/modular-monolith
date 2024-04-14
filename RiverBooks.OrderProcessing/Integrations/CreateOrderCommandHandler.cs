using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using Serilog;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  private readonly IOrderRepository _orderRepository;
  private readonly ILogger _logger;
  private readonly IOrderAddressCache _addressCache;

  public CreateOrderCommandHandler(IOrderRepository orderRepository,
    ILogger logger,
    IOrderAddressCache addressCache)
  {
    _orderRepository = orderRepository;
    _logger = logger;
    _addressCache = addressCache;
  }
  
  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(oi => new OrderItem(oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description));
    
    var shippingAddress = await _addressCache.GetById(request.ShippingAddressId);
    var billingAddress = await _addressCache.GetById(request.BillingAddressId);

    var newOrder = Order.Factory.Create(
      request.UserId,
      shippingAddress.Value.Address,
      billingAddress.Value.Address,
      items);

    await _orderRepository.Add(newOrder);
    await _orderRepository.SaveChanges();
    
    _logger.Information("New Order Created! {OrderId}", newOrder.Id);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
