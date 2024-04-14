namespace RiverBooks.Users;

public interface IHaveDomainEvents
{
  IEnumerable<DomainEventBase> DomainEvents { get; }
  void ClearDomainEvents();
}
