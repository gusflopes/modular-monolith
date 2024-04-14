using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.UserEndpoints;
using Serilog;

namespace RiverBooks.Users.UseCases;

public record AddAddressToUserCommand(string EmailAddress, string Street1, string Street2, string City, string State, string ZipCode, string Country) : IRequest<Result>;

public class AddAdressToUserHandler : IRequestHandler<AddAddressToUserCommand, Result>
{
  private readonly IApplicationUserRepository _userRepository;
  private readonly ILogger<AddAdressToUserHandler> _logger;

  public AddAdressToUserHandler(IApplicationUserRepository userRepository, ILogger<AddAdressToUserHandler> logger)
  {
    _userRepository = userRepository;
    _logger = logger;
  }
  
  public async Task<Result> Handle(AddAddressToUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithAddressesByEmail(request.EmailAddress);
    if (user is null)
    {
      return Result.Unauthorized();
    }
    
    var addressToAdd = new Address(request.Street1, request.Street2, request.City, request.State, request.ZipCode, request.Country);

    var userAddress = user.AddAddress(addressToAdd);
    await _userRepository.SaveChanges();
    
    _logger.LogInformation("[UseCase] Added address {address} to user {email} (Total: {total})", userAddress.StreetAddress, request.EmailAddress, user.Addresses.Count);

    return Result.Success();
  }
}
