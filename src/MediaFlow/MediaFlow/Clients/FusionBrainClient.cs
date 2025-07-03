using System.Net;
using System.Text;
using System.Text.Json;

namespace MediaFlow.Clients
{
	public class FusionBrainClient : IAiImageClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl;

		public FusionBrainClient()
		{
			_baseUrl = "https://api-key.fusionbrain.ai/";
			var apiKey = "{your-key}";
			var secretKey = "{your-secret}";

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

			_httpClient = new HttpClient(handler);

			_httpClient.DefaultRequestHeaders.Remove("X-Key");
			_httpClient.DefaultRequestHeaders.Remove("X-Secret");
			_httpClient.DefaultRequestHeaders.Add("X-Key", $"Key {apiKey}");
			_httpClient.DefaultRequestHeaders.Add("X-Secret", $"Secret {secretKey}");
		}

		public async Task<string> GenerateImageAsync(string prompt)
		{
			var pipelineId = await GetPipelineAsync();
			Console.WriteLine($"Pipeline ID: {pipelineId}");

			var uuid = await GenerateAsync(prompt, pipelineId);
			Console.WriteLine($"Generation UUID: {uuid}");

			var files = await CheckGenerationAsync(uuid);

			if (files != null)
			{
				Console.WriteLine("Generated file URLs:");
				//foreach (var url in files)
				//	Console.WriteLine(url);

				Console.WriteLine(files[0]);

				return files[0];
			}
			else
			{
				Console.WriteLine("Image generation timed out.");

				return string.Empty;
			}
		}

		private async Task<string> GetPipelineAsync()
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}key/api/v1/pipelines");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			// Assumes array and takes first element id
			var pipelineId = root[0].GetProperty("id").GetString();
			return pipelineId ?? throw new Exception("Pipeline id not found");
		}

		private async Task<string> GenerateAsync(string prompt, string pipeline, int images = 1, int width = 576, int height = 1024)
		{
			var paramsObj = new
			{
				type = "GENERATE",
				numImages = images,
				width = width,
				height = height,
				generateParams = new { query = prompt }
			};

			var jsonParams = JsonSerializer.Serialize(paramsObj);

			using var content = new MultipartFormDataContent
		{
			{ new StringContent(pipeline), "pipeline_id" },
			{ new StringContent(jsonParams, Encoding.UTF8, "application/json"), "params" }
		};

			var response = await _httpClient.PostAsync($"{_baseUrl}key/api/v1/pipeline/run", content);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			var uuid = root.GetProperty("uuid").GetString();
			return uuid ?? throw new Exception("UUID not returned");
		}

		private async Task<List<string>?> CheckGenerationAsync(string uuid, int attempts = 15, int delaySeconds = 3)
		{
			while (attempts > 0)
			{
				var response = await _httpClient.GetAsync($"{_baseUrl}key/api/v1/pipeline/status/{uuid}");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(json);
				var root = doc.RootElement;

				var status = root.GetProperty("status").GetString();
				if (status == "DONE")
				{
					var files = new List<string>();
					var filesJson = root.GetProperty("result").GetProperty("files");
					foreach (var fileElem in filesJson.EnumerateArray())
					{
						files.Add(fileElem.GetString() ?? "");
					}
					return files;
				}

				attempts--;
				if (attempts > 0)
					await Task.Delay(delaySeconds * 1000);
			}

			return null;
		}
	}
}
