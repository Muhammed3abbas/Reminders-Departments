namespace RingoMediaReminder.Services
{
    public interface IMailingServices
    {
        Task SendEmailAsync(IList<string> mailTos, string subject, string body);
    }
}
