namespace RiverBooks.EmailSending.EmailBackgroundService;

internal interface ISendEmail
{
  Task SendEmail(string to, string from, string subject, string body);
}
