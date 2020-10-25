using System;
using System.Diagnostics;
using RedOwl.Engine;

namespace RedOwl.Editor
{
    public static class Git
    {
        public static string BuildVersion => Run(@"describe --tags --long --abbrev=12 --always --match ""v[0-9]*""");

        public static string Branch => Run(@"rev-parse --abbrev-ref HEAD");

        public static string Status => Run(@"status --porcelain");

        /// <summary>
        /// Runs git.exe with the specified arguments and returns the output.
        /// </summary>
        public static string Run(string arguments)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo("git", arguments)
                {
                    CreateNoWindow = true, // We want no visible pop-ups
                    UseShellExecute = false, // Allows us to redirect input, output and error streams
                    RedirectStandardOutput = true, // Allows us to read the output stream
                    RedirectStandardError = true // Allows us to read the error stream
                }
            };

            try
            {
                process.Start();
            }
            catch (Exception)
            {
                // For now just assume its failed cause it can't find git.
                Log.Error("Git is not set-up correctly, required to be on PATH, and to be a git project.");
                throw;
            }

            string output = process.StandardOutput.ReadToEnd();
            string errorOutput = process.StandardError.ReadToEnd();

            process.WaitForExit(); // Make sure we wait till the process has fully finished.
            process.Close(); // Close the process ensuring it frees it resources.

            // Check for failure due to no git setup in the project itself or other fatal errors from git.
            if (output.Contains("fatal") || output == "no-git" || output == "")
            {
                throw new Exception($"Command: git {arguments} Failed\n{output}\n\n{errorOutput}");
            }

            if (errorOutput != "")
            {
                Log.Error("Git Error: " + errorOutput);
            }

            return output.Trim();
        }
    }
}
