using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.CartEndpoints;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress) : IRequest<Result>;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, Result>
{
  private readonly IApplicationUserRepository _userRepository;

  public AddItemToCartHandler(IApplicationUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmail(request.EmailAddress);

    if (user is null)
    {
      return Result.Unauthorized();
    }
    
    // TODO: Get description and price from the Books Module
    var newCartItem = new CartItem(request.BookId, "description", request.Quantity, 1.00m);

    await _userRepository.SaveChanges();
    
    return Result.Success();
  }
}
