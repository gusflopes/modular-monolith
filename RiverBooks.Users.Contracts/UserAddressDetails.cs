namespace RiverBooks.Users.Contracts;

public record UserAddressDetails(
  Guid UserId,
  Guid AddressId,
  string Street,
  string Street2,
  string City,
  string State,
  string ZipCode,
  string Country);
