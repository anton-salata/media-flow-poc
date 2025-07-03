using System.Diagnostics;


namespace MediaFlow.Services
{

	public class VideoGeneratorService
	{
		private readonly string _scriptsFolder = "Scripts";
		private readonly string _outputFolder = Path.Combine("Data", "Videos");
		private readonly string _tempFolder = Path.Combine("Data", "Temp");

		public VideoGeneratorService()
		{
			Directory.CreateDirectory(_outputFolder);
			Directory.CreateDirectory(_tempFolder);
		}

		public string GenerateVideo(string shortQuote, string longQuote, byte[] base64Image)
		{
			// Sanitize filename
			var safeName = string.Concat(shortQuote.Split(Path.GetInvalidFileNameChars()));
			var baseFileName = Path.Combine(_tempFolder, safeName);

			var textPath = baseFileName + "_input.txt";
			var audioPath = baseFileName + "_audio.wav";
			var imagePath = baseFileName + "_image.png";
			var videoPath = Path.Combine(_outputFolder, safeName + ".mp4");

			// Save the long quote to a file
			File.WriteAllText(textPath, longQuote);

			// Decode and save base64 image
			SaveBase64Image(base64Image, imagePath);

			Console.WriteLine("[INFO] Starting audio generation...");
			RunPythonScript("generate_audio.py", $"\"{textPath}\" \"{audioPath}\"");


			Console.WriteLine("[INFO] Starting video generation...");
			RunPythonScript("generate_video.py",
				$"--image \"{imagePath}\" --text \"{longQuote}\" --audio \"{audioPath}\" --output \"{videoPath}\"");

			Console.WriteLine($"[✓] Video successfully saved to: {videoPath}");
			return videoPath;
		}

		private void SaveBase64Image(byte[] base64Image, string outputPath)
		{
			try
			{
				//var base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1); // Skip header
				//var imageBytes = Convert.FromBase64String(base64Image);
				File.WriteAllBytes(outputPath, base64Image);
				Console.WriteLine($"[INFO] Image saved to: {outputPath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine("[ERROR] Failed to decode base64 image: " + ex.Message);
				throw;
			}
		}

		private void RunPythonScript(string scriptName, string arguments)
		{
			var scriptPath = Path.Combine(_scriptsFolder, scriptName);

			var start = new ProcessStartInfo
			{
				FileName = "python",
				Arguments = $"\"{scriptPath}\" {arguments}",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};

			using var process = new Process { StartInfo = start };
			process.OutputDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine("[PY] " + e.Data); };
			process.ErrorDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.Error.WriteLine("[PY-ERR] " + e.Data); };

			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new Exception($"Script {scriptName} failed with exit code {process.ExitCode}");
		}
	}

}
