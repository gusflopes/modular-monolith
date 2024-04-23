using Ardalis.Result;
using MediatR;
using MongoDB.Driver;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending.Integrations;
internal interface IQueueEmailsInOutboxService
{
  Task QueueEmailForSending(EmailOutboxEntity entity);
}

internal class MongoDbQueueEmailOutboxService : IQueueEmailsInOutboxService
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection;

  public MongoDbQueueEmailOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection)
  {
    _emailCollection = emailCollection;
  }
  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }
}

internal class QueueEmailInOutboxSendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<Guid>>
{
  private readonly IQueueEmailsInOutboxService _outboxService;
  public QueueEmailInOutboxSendEmailCommandHandler(IQueueEmailsInOutboxService outboxService)
  {
    _outboxService = outboxService;
  }
  public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
  {
    var newEntity = new EmailOutboxEntity
    {
      To = request.To,
      From = request.From,
      Subject = request.Subject,
      Body = request.Body
    };

    await _outboxService.QueueEmailForSending(newEntity);
    
    return newEntity.Id;
  }
}
