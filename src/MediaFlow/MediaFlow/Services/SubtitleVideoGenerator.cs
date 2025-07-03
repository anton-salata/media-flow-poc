using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MediaFlow.Services
{
	public class SubtitleVideoGenerator
	{
		public void GenerateVideo(
			string imagePath,
			string srtPath,
			string audioPath,
			string musicPath,
			string outputPath)
		{
			var scriptPath = Path.Combine("Scripts", "video_with_styled_subs.py");

			var args = $"--image \"{imagePath}\" --srt \"{srtPath}\" --audio \"{audioPath}\" --output \"{outputPath}\"";

			if (!string.IsNullOrEmpty(musicPath))
			{
				args += $" --music \"{musicPath}\"";
			}

			RunPythonScript(scriptPath, args);
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
