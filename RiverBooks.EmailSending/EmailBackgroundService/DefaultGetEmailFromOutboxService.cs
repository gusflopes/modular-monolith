using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;
internal interface IGetEmailsFromOutboxService
{
  Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}

internal class MongoDbGetEmailsFromOutboxService : IGetEmailsFromOutboxService
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection;

  public MongoDbGetEmailsFromOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection)
  {
    _emailCollection = emailCollection;
  }

  public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
  {
    var filter = Builders<EmailOutboxEntity>.Filter.Eq(entity =>
      entity.DateTimeUtcProcessed, null);
    var unsentEmailEntity = await _emailCollection
      .Find(filter)
      .FirstOrDefaultAsync();

    if (unsentEmailEntity == null) return Result.NotFound();

    return unsentEmailEntity;
  }
}

