namespace RiverBooks.Users.Data;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}
