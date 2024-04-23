namespace RiverBooks.EmailSending;

internal interface ISendEmail
{
  Task SendEmail(string to, string from, string subject, string body);
}
