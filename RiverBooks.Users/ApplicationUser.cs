using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser, IHaveDomainEvents
{
  public string FullName { get; set; } = string.Empty;
  
  private readonly List<CartItem> _cartItems = new();
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
  
  private readonly List<UserStreetAddress> _addresses = new();
  public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();
  
  private List<DomainEventBase> _domainEvents = new();
  [NotMapped] public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();
  
  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();

  internal UserStreetAddress AddAddress(Address address)
  {
    Guard.Against.Null(address);

    var existingAddress = _addresses.SingleOrDefault(a => a.StreetAddress == address);
    if (existingAddress != null)
    {
      return existingAddress;
    }

    var newAddress = new UserStreetAddress(Id, address);
    _addresses.Add(newAddress);

    // Raise an AddressCreatedEvent (DomainEvent)
    var domainEvent = new AddressAddedEvent(newAddress);
    RegisterDomainEvent(domainEvent);
    
    return newAddress;
  }
  
  public void AddItemToCart(CartItem item)
  {
    Guard.Against.Null(item);
    
    var existingItem = _cartItems.SingleOrDefault(ci => ci.BookId == item.BookId);
    if (existingItem != null)
    {
      existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
      existingItem.UpdateDescription(item.Description);
      existingItem.UpdateUnitPrice(item.UnitPrice);
      return;
    }
    _cartItems.Add(item);
  }
  
  internal void ClearCart()
  {
    _cartItems.Clear();
  }
}

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
