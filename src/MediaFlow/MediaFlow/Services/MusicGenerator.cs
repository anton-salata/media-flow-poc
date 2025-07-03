using System.Diagnostics;

namespace MediaFlow.Services
{
	public class MusicGenerator
	{
		public void GenerateMusic(string outputFile, string prompt, int duration)
		{
			var scriptPath = Path.Combine("Scripts", "generate_music.py");

			RunPythonScript(scriptPath, $"--output \"{outputFile}\" --prompt \"{prompt}\" --duration \"{duration}\"");
		}


		private void RunPythonScript(string scriptPath, string arguments)
		{
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
				throw new Exception($"Script {scriptPath} failed with exit code {process.ExitCode}");
		}
	}
}
