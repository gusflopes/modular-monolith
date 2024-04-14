using Ardalis.GuardClauses;

namespace RiverBooks.Users;

public class UserStreetAddress
{
  public UserStreetAddress(string userId, Address streetAddress)
  {
    UserId = Guard.Against.NullOrEmpty(userId);
    StreetAddress = Guard.Against.Null(streetAddress);
  }

  public UserStreetAddress()
  {
    // EFCore
  }
  
  public Guid Id { get;private set; } = Guid.NewGuid();
  public string UserId { get; private set; } = String.Empty;
  public Address StreetAddress { get; private set; } = default!;
}
