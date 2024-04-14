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
