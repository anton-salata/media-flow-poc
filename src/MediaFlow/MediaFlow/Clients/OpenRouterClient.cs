using MediaFlow.Clients.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MediaFlow.Clients
{
	public class OpenRouterClient : IAiChatClient
	{
		public async Task<string> GetAnswer(string prompt)
		{
			{
				var proxy = new WebProxy("{proxy}")
				{
					UseDefaultCredentials = true
				};

				var handler = new HttpClientHandler
				{
					Proxy = proxy,
					UseProxy = true,
					UseDefaultCredentials = true
				};

				using var httpClient = new HttpClient(handler);

				var apiKey = "{your-key}";
				var apiUrl = "https://openrouter.ai/api/v1/chat/completions";

				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

				var requestData = new
				{
					model = "google/gemma-3n-e4b-it:free",
					messages = new[] { new { role = "user", content = prompt } }
				};

				var requestJson = JsonSerializer.Serialize(requestData);
				var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var response = await httpClient.PostAsync(apiUrl, content);
				var responseJson = await response.Content.ReadAsStringAsync();

				using JsonDocument doc = JsonDocument.Parse(responseJson);

				var aiChatResponse = doc
					.RootElement
					.GetProperty("choices")[0]
					.GetProperty("message")
					.GetProperty("content")
					.GetString();

				return aiChatResponse;
			}
		}
	}
}
