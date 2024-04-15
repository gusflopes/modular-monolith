using FluentValidation;

namespace RiverBooks.Users.UseCases;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
  public AddItemToCartCommandValidator()
  {
    RuleFor(x => x.BookId).NotEmpty().WithMessage("Not a valid BookId");
    RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
    RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithMessage("EmailAddress is required.");
  }
}
