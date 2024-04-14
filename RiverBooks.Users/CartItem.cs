using Ardalis.GuardClauses;

namespace RiverBooks.Users;

public class CartItem
{
  public CartItem(Guid bookId, string description, int quantity, decimal unitPrice)
  {
    BookId = Guard.Against.Default(bookId);
    Description = Guard.Against.NullOrEmpty(description);
    Quantity = Guard.Against.NegativeOrZero(quantity);
    UnitPrice = Guard.Against.NegativeOrZero(unitPrice);
  }

  public CartItem()
  {
    // EFCore
  }

  public Guid Id { get; private set; } = Guid.NewGuid();
  public Guid BookId { get; private set; }
  public string Description { get; private set; } = string.Empty;
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }
  
  internal void UpdateQuantity(int quantity)
  {
    Quantity = Guard.Against.NegativeOrZero(quantity);
  }

  internal void UpdateDescription(string description)
  {
    Description = Guard.Against.NullOrEmpty(description);
  }

  internal void UpdateUnitPrice(decimal unitPrice)
  {
    UnitPrice = Guard.Against.NegativeOrZero(unitPrice);
  }
}
