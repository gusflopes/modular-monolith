using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressCacheUpdatingNewUserAddressHandler : INotificationHandler<NewUserAddressAddedIntegrationEvent>
{
  private readonly IOrderAddressCache _addressCache;
  private readonly ILogger<AddressCacheUpdatingNewUserAddressHandler> _logger;

  public AddressCacheUpdatingNewUserAddressHandler(IOrderAddressCache addressCache,
    ILogger<AddressCacheUpdatingNewUserAddressHandler> logger)
  {
    _addressCache = addressCache;
    _logger = logger;
  }

  public async Task Handle(NewUserAddressAddedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    var orderAddress = new OrderAddress(
      notification.AddressDetails.AddressId,
      new Address(
        notification.AddressDetails.Street,
        notification.AddressDetails.Street2,
        notification.AddressDetails.City,
        notification.AddressDetails.State,
        notification.AddressDetails.ZipCode,
        notification.AddressDetails.Country
      )
    );

    await _addressCache.Store(orderAddress);

    _logger.LogInformation("Cache updated with new address {address}", orderAddress);
}
}
