using MediaFlow.Clients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Threading.Tasks;

namespace MediaFlow.Clients
{
	public class DeepSeekClient : IAiChatClient
	{
		public async Task<string> GetAnswer(string prompt)
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
			var apiUrl = "https://api.deepseek.com/chat/completions";

			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			var requestData = new
			{
				model = "deepseek-chat",
				messages = new[] { new { role = "user", content = "Hello!" } },
				stream = false
			};

			var json = System.Text.Json.JsonSerializer.Serialize(requestData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync(apiUrl, content);
			var responseJson = await response.Content.ReadAsStringAsync();

			return responseJson;
		}
	}
}
