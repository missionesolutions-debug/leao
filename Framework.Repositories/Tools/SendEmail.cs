using RestSharp;
using System.Collections.Generic;

public class SendEmail
{
	public Boolean Send(String Body, String emailToSend, String titulo, String assunto)
	{
		SendSendblue(emailToSend, Body, assunto);
		return true;
	}

	public Boolean SendSendblue(string user, String body, String assunto)
	{
		var client = new RestClient("https://api.brevo.com/v3/smtp/email");
		var request = new RestRequest("", Method.Post);
		request.AddHeader("accept", "application/json");
		request.AddHeader("api-key", "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");
		request.AddHeader("Content-Type", "application/json");

		RequestSendblue requestBody = new()
		{
			subject = assunto,
			htmlContent = body,
			sender = new()
			{
				email = "noreply@codienova.com.br",
				name = "MG7"
			},
			to = new()
			{
				new()
				{
					email = user,
					name = "Recipient"
				}
			}
		};

		request.AddParameter("application/json", requestBody, ParameterType.RequestBody);

		var response = client.Execute(request);

		if (!response.IsSuccessful)
			throw new Exception($"Erro ao enviar o email: {response.ErrorMessage}");

		return true;
	}
}

public class RequestSendblue
{
	public SenderSB sender { get; set; }
	public List<ToSB> to { get; set; }
	public string subject { get; set; }
	public string htmlContent { get; set; }
}

public class SenderSB
{
	public string name { get; set; }
	public string email { get; set; }
}

public class ToSB
{
	public string email { get; set; }
	public string name { get; set; }
}
