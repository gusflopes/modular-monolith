using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;

namespace RiverBooks.Users.Integrations;

public class UserAddressDetailsByIdQueryHandler : IRequestHandler<UserAddressDetailsByIdQuery, Result<UserAddressDetails>>
{
  private readonly IReadOnlyUserStreetAddressRepository _addressRepository;

  public UserAddressDetailsByIdQueryHandler(IReadOnlyUserStreetAddressRepository addressRepository)
  {
    _addressRepository = addressRepository;
  }
  
  
  public async Task<Result<UserAddressDetails>> Handle(UserAddressDetailsByIdQuery request, CancellationToken cancellationToken)
  {
    var address = await _addressRepository.GetById(request.AddressId);

    if (address is null) { return Result.NotFound();}
    
    Guid userId = Guid.Parse(address.UserId);
    
    var details = new UserAddressDetails(
      userId,
      address.Id,
      address.StreetAddress.Street,
      address.StreetAddress.Street2,
      address.StreetAddress.City,
      address.StreetAddress.State,
      address.StreetAddress.ZipCode,
      address.StreetAddress.Country);

    return details;
  }
}
