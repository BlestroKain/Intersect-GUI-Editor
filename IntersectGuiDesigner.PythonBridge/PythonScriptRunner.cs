using System.Diagnostics;
using System.Linq;

namespace IntersectGuiDesigner.PythonBridge;

public sealed class PythonRunResult
{
    public int ExitCode { get; init; }
    public string StandardOutput { get; init; } = string.Empty;
    public string StandardError { get; init; } = string.Empty;
}

public static class PythonScriptRunner
{
    public static PythonRunResult Run(string scriptPath, IEnumerable<string> arguments, string? workingDirectory = null, string? pythonExecutable = null)
    {
        if (string.IsNullOrWhiteSpace(scriptPath))
        {
            throw new ArgumentException("Script path cannot be null or empty.", nameof(scriptPath));
        }

        var pythonExe = string.IsNullOrWhiteSpace(pythonExecutable) ? "python" : pythonExecutable;
        var fullArguments = new List<string> { scriptPath };
        fullArguments.AddRange(arguments ?? Array.Empty<string>());

        var startInfo = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = string.Join(" ", fullArguments.Select(QuoteArgument)),
            WorkingDirectory = string.IsNullOrWhiteSpace(workingDirectory) ? Environment.CurrentDirectory : workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return new PythonRunResult
        {
            ExitCode = process.ExitCode,
            StandardOutput = standardOutput,
            StandardError = standardError
        };
    }

    private static string QuoteArgument(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            return "\"\"";
        }

        if (!argument.Any(char.IsWhiteSpace) && !argument.Contains('"'))
        {
            return argument;
        }

        var escaped = argument.Replace("\"", "\\\"");
        return $"\"{escaped}\"";
    }
}
