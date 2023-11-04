using Microsoft.Playwright;
using System.Diagnostics;

namespace SpecFlowProject.TestSupport
{
    public static class Helpers
    {
        public static async Task<string> TakeScreenshot(IPage page)
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            var title = await page.TitleAsync();
            var path = $"Screenshots/{date}_{title}.png";
            var so = new PageScreenshotOptions()
            {
                Path = path,
                FullPage = true,
            };
            await page.ScreenshotAsync(so);

            return path;
        }
        
        public static string RunCommand(string command, string arguments)
        {
            // Get the user's home directory
            var userHomeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Construct the path to .dotnet/tools
            var dotnetToolsPath = Path.Combine(userHomeDirectory, ".dotnet", "tools");

            // Construct the full command path
            var fullCommandPath = Path.Combine(dotnetToolsPath, command);

            // Create a new process
            var process = new Process();

            // Configure the process start info
            var startInfo = new ProcessStartInfo
            {
                FileName = fullCommandPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;

            // Start the process
            process.Start();

            // Capture and return the output
            var output = process.StandardOutput.ReadToEnd();
            var errorOutput = process.StandardError.ReadToEnd();

            process.WaitForExit();
            process.Close();

            if (!string.IsNullOrWhiteSpace(errorOutput))
            {
                throw new Exception($"Error running command: {errorOutput}");
            }

            return output;
        }
    }
}