namespace RiverBooks.EmailSending;

internal interface IOutboxService
{
  Task QueueEmailForSending(EmailOutboxEntity entity);
}
