using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MediaFlow.Services
{
	public class SubtitleGenerator
	{
		public static void GenerateSubtitles(string audioPath, string transcriptPath, string outputSrtPath)
		{
			var pythonExe = "python";

			var scriptPath = Path.Combine("Scripts", "generate_subtitles.py");

			var args = $"\"{scriptPath}\" --audio \"{audioPath}\" --text \"{transcriptPath}\" --srt \"{outputSrtPath}\"";

			var psi = new ProcessStartInfo
			{
				FileName = pythonExe,
				Arguments = args,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};

			using (var process = Process.Start(psi))
			{
				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (!string.IsNullOrWhiteSpace(output))
					Console.WriteLine(output);

				if (!string.IsNullOrWhiteSpace(error))
					Console.Error.WriteLine(error);
			}
		}
	}

}
