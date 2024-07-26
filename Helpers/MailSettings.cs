namespace RingoMediaReminder.Helpers
{
    public class MailSettings
    {
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int? Port { get; set; }

        public string? SmtpServer { get; set; }
        public string? Username { get; set; }
        public string? FromEmail { get; set; }
    }
}
