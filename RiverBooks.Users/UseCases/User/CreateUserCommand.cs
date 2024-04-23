using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.Users.UseCases;

internal record CreateUserCommand(string Email, string Password) : IRequest<Result>; 

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result> 
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IMediator _mediator;

  public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
  {
    _userManager = userManager;
    _mediator = mediator;
  }
  
  public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    var newUser = new ApplicationUser { Email = command.Email, UserName = command.Email };

    var result = await _userManager.CreateAsync(newUser, command.Password);

    if (!result.Succeeded)
      return Result.Error(result.Errors.Select(e => e.Description).ToArray());
    
    // Send welcome email
    var sendEmailCommand = new SendEmailCommand
    {
      To = command.Email,
      From = "donotreply@hublaw.app",
      Subject = "Welcome to RiverBooks!",
      Body = "Thank you for registering."
    };

    _ = await _mediator.Send(sendEmailCommand);
    
    return Result.Success();

  }
}
