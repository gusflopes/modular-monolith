namespace RiverBooks.Users.UseCases;

public class AddressListResponse
{
  public List<UserAddressDto> Addresses { get; set; } = new();
}
