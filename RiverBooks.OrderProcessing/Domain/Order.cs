﻿using System.ComponentModel.DataAnnotations.Schema;
using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

internal class Order : IHaveDomainEvents
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public Guid UserId { get; private set; }
  public Address ShippingAddress { get; private set; } = default!;
  public Address BillingAddress { get; private set; } = default!;
  private readonly List<OrderItem> _orderItems = new();
  public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
  public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;
  public void AddOrderItem(OrderItem item) => _orderItems.Add(item);
  
  private List<DomainEventBase> _domainEvents = new();
  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  [NotMapped] public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();
  void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();


  // This nested class will be able to access all the private members of the Order class
  internal static class Factory
  {
    public static Order Create(Guid userId, Address shippingAddress, Address billingAddress, IEnumerable<OrderItem> orderItems)
    {
      var order = new Order();
      order.UserId = userId;
      order.ShippingAddress = shippingAddress;
      order.BillingAddress = billingAddress;
      foreach (var item in orderItems)
      {
        order.AddOrderItem(item);
      }

      var createdEvent = new OrderCreatedEvent(order);
      order.RegisterDomainEvent(createdEvent);
      
      return order;
    }
  }
}
