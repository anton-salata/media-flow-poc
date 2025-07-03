using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MediaFlow.Services
{
	

	public class WhisperWrapper
	{
		public static void GenerateSubtitles(string audioPath, string srtOutputPath)
		{
			var scriptPath = Path.Combine("Scripts", "whisper_transcribe.py");

			var psi = new ProcessStartInfo
			{
				FileName = "python",
				Arguments = $"\"{scriptPath}\" \"{audioPath}\" \"{srtOutputPath}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using (var process = new Process())
			{
				process.StartInfo = psi;
				process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
				process.ErrorDataReceived += (sender, e) => Console.Error.WriteLine(e.Data);

				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();

				if (process.ExitCode != 0)
					throw new Exception("Whisper transcription failed.");
			}

			Console.WriteLine($"Subtitles saved to: {srtOutputPath}");
		}
	}

}
