using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending;

internal class QueueEmailInOutboxSendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<Guid>>
{
  private readonly IOutboxService _outboxService;
  // private readonly ISendEmail _emailSender;

  public QueueEmailInOutboxSendEmailCommandHandler(IOutboxService outboxService)
  {
    _outboxService = outboxService;
    // _emailSender = emailSender;
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
