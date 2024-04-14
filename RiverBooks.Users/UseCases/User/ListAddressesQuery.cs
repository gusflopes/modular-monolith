using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases;

public record ListAddressesQuery(string EmailAddress) : IRequest<Result<List<UserAddressDto>>>;

internal class ListAddressesQueryHandler : IRequestHandler<ListAddressesQuery, Result<List<UserAddressDto>>>
{
  private readonly IApplicationUserRepository _userRepository;

  public ListAddressesQueryHandler(IApplicationUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<Result<List<UserAddressDto>>> Handle(ListAddressesQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithAddressesByEmail(request.EmailAddress);
    if (user == null)
    {
      return Result.Unauthorized();
    }

    return user!.Addresses!
      .Select(ua => new UserAddressDto(
        ua.Id,
        ua.StreetAddress.Street,
        ua.StreetAddress.Street2,
        ua.StreetAddress.City,
        ua.StreetAddress.State,
        ua.StreetAddress.ZipCode,
        ua.StreetAddress.Country))
      .ToList();

  }
}
