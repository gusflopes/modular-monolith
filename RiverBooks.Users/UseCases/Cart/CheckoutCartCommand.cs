using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;

namespace RiverBooks.Users.UseCases;

public record CheckoutCartCommand(string EmailAddress, Guid ShippingAddressId, Guid BillingAddressId) : IRequest<Result<Guid>>;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
  private readonly IMediator _mediator;
  private readonly IApplicationUserRepository _userRepository;

  public CheckoutCartCommandHandler(IMediator mediator, IApplicationUserRepository userRepository)
  {
    _mediator = mediator;
    _userRepository = userRepository;
  }
  public async Task<Result<Guid>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmail(request.EmailAddress);

    if (user is null)
    {
      return Result.Unauthorized();
    }
    
    var items = user.CartItems.Select(item => new OrderItemDetails(item.BookId, item.Quantity, item.UnitPrice, item.Description)).ToList();
    
    var createOrderCommand = new CreateOrderCommand(Guid.Parse(user.Id), request.ShippingAddressId, request.BillingAddressId, items);

    // TODO: Consider replacing with a message-based approach for perf reasons 
    var result = await _mediator.Send(createOrderCommand);

    if (!result.IsSuccess)
    {
      // Change from a Result<OrderDetailsResponse> to Result<Guid>
      return result.Map(x => x.OrderId);
    }

    user.ClearCart();
    await _userRepository.SaveChanges();
    return Result.Success(result.Value.OrderId);
    // send email to Customer
  }
}

