using System.Text;
using System.Text.Json;

namespace Framework.Infrastructure.Mailing.MessageProvider
{
    public class EmailService : IEmailService
    {
        private readonly string _emailProviderApiUrl = "https://api.emailprovider.com/send";
        private readonly string _apiKey = "YourAPIKeyHere"; // Securely load this from configuration

        public EmailService()
        {
            
        }

        public async Task SendEmailByMessageProvider(string accountTemplateKeyId, Dictionary<string, string> parameters)
        {

            HttpClient _httpClient = new HttpClient();

            // Prepare the payload for the email provider API
            var emailRequestPayload = new
            {
                templateId = accountTemplateKeyId,
                recipient = parameters["email"],
                parameters = parameters
            };

            var jsonPayload = JsonSerializer.Serialize(emailRequestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Set API key in headers if required by the provider
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            // Send request to the external email API
            var response = await _httpClient.PostAsync(_emailProviderApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {errorResponse}");
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, string token, string link, string userName, string apiKey)
        {
            var parameters = new Dictionary<string, string>
        {
            { "email", email },
            { "link", $"https://yourapp.com/reset-password?token={token}" },
            { "name", userName } // If you have user's name, add it here
        };

            await SendEmailByMessageProvider(apiKey, parameters);
        }

    }
}
