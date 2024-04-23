using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal class MongoDbOutboxService : IOutboxService
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection;

  public MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection)
  {
    _emailCollection = emailCollection;
  }

  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }
}
