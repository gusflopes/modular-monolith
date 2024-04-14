namespace RiverBooks.Users.Integrations;

public interface IReadOnlyUserStreetAddressRepository
{
  Task<UserStreetAddress?> GetById(Guid addressId);
}
