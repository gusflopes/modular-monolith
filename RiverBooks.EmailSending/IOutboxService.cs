using Ardalis.Result;

namespace RiverBooks.EmailSending;

internal interface IOutboxService
{
  Task QueueEmailForSending(EmailOutboxEntity entity);
  Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}
