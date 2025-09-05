namespace Framework.Infrastructure.Mailing.MessageProvider
{
    public interface IEmailService
    {
        Task SendEmailByMessageProvider(string accountTemplateKeyId, Dictionary<string, string> parameters);
        Task SendPasswordResetEmailAsync(string email, string token, string link, string userName, string apiKey);
    }
}
