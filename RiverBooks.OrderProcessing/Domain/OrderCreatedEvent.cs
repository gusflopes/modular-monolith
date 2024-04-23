using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.SharedKernel;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Domain;

internal class OrderCreatedEvent : DomainEventBase
{
  public Order Order {get;}
  
  public OrderCreatedEvent(Order order)
  {
    Order = order;
  }
}

internal class SendConfirmationEmailOrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
  private readonly IMediator _mediator;

  public SendConfirmationEmailOrderCreatedEventHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  
  public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
  {
    var userByIdQuery = new UserDetailsByIdQuery(notification.Order.UserId);

    var result = await _mediator.Send(userByIdQuery, cancellationToken);

    if (!result.IsSuccess)
    {
      // TODO: Log the error
      return;
    }

    string userEmail = result.Value.EmailAddress;
    
    var command = new SendEmailCommand
    {
      To = userEmail,
      From = "noreply@test.com",
      Subject = "Your RiverBooks purchase",
      Body = $"You bought {notification.Order.OrderItems.Count} items."
    };
    
    Guid emailId = await _mediator.Send(command);
    
    // TODO: Do something with the emailId
    
  }
}
