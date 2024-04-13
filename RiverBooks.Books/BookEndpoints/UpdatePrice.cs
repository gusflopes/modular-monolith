using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books;

public record UpdateBookPriceRequest(Guid Id, decimal NewPrice);

public class UpdateBookPriceRequestValidator : Validator<UpdateBookPriceRequest>
{
  public UpdateBookPriceRequestValidator()
  {
    RuleFor(x => x.Id)
      .NotNull()
      .NotEqual(Guid.Empty)
      .WithMessage("A book id is required.");
    
    RuleFor(x => x.NewPrice)
      .GreaterThan(0)
      .WithMessage("Price must be greater than 0.");
  }
}

internal class UpdateBookPriceEndpoint(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  public override void Configure()
  {
    Post("/books/{Id}/pricehistory");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
  {
    // TODO: Handle not Found
    await _bookService.UpdateBookPrice(req.Id, req.NewPrice);
    var updatedBook = await _bookService.GetBookById(req.Id);
    await SendAsync(updatedBook);
  }
}
