using MediaFlow.Clients.Interfaces;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Net;

namespace MediaFlow.Clients
{
	public class ChatGptClient : IAiChatClient
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
				UseProxy = false,
				UseDefaultCredentials = true
			};

			using var httpClient = new HttpClient(handler);

			//var ip = await httpClient.GetStringAsync("https://api.ipify.org");
			//Console.WriteLine($"Public IP: {ip}");

			var transport = new HttpClientPipelineTransport(httpClient);

			var options = new OpenAIClientOptions
			{
				Transport = transport
			};

			var client = new ChatClient("gpt-4o", new ApiKeyCredential("{your-key}"), options);

			var completion = client.CompleteChat(prompt);

			return completion.Value.Content[0].Text;
		}
	}
}
