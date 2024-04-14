using RiverBooks.SharedKernel;

namespace RiverBooks.Users.Contracts;

public record NewUserAddressAddedIntegrationEvent(UserAddressDetails AddressDetails) : IntegrationEventBase;
