﻿using System.Diagnostics;


namespace MediaFlow.Services
{

	public class NarrationGeneratorV2
	{
		private readonly string _scriptsFolder = "Scripts";
		//private readonly string _outputFolder = Path.Combine("Data", "Videos");
		private readonly string _tempFolder = Path.Combine("Data", "Temp");

		public NarrationGeneratorV2()
		{
			//Directory.CreateDirectory(_outputFolder);
			Directory.CreateDirectory(_tempFolder);
		}

		public void GenerateNarration(string textFile, string outputFile)
		{
			Console.WriteLine("[INFO] Starting audio generation...");
			RunPythonScript("generate_tts_coqui.py", $"--text_file \"{textFile}\" --output \"{outputFile}\"");

			Console.WriteLine($"[✓] Audio successfully saved to: {outputFile}");
		}		

		private void RunPythonScript(string scriptName, string arguments)
		{
			var scriptPath = Path.Combine(_scriptsFolder, scriptName);

			var start = new ProcessStartInfo
			{
				FileName = "C:\\Users\\UserName\\AppData\\Local\\Programs\\Python\\Python311\\python.exe",
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
