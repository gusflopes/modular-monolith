using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases;

internal record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
  private readonly IApplicationUserRepository _userRepository;

  public GetUserByIdQueryHandler(IApplicationUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserById(request.UserId);
    if (user is null)
      return Result.NotFound();

    return new UserDto(Guid.Parse(user!.Id), user.Email!);
  }
}
