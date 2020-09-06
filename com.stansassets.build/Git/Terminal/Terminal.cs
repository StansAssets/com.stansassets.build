using System.Diagnostics;

namespace StansAssets.Git
{
    class Terminal : ITerminal
    {
        readonly string m_Terminal;
        readonly string m_Path;
        readonly string m_Args;

        public Terminal(string terminal, string path, string args = default)
        {
            m_Terminal = terminal;
            m_Path = path;
            m_Args = args;
        }

        public (string output, string error) Call(params string[] commands)
        {
            var processStartInfo = new ProcessStartInfo(m_Terminal, m_Args)
            {
                WorkingDirectory = m_Path,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var output = string.Empty;
            var error = string.Empty;

            using (var process = new Process() { StartInfo = processStartInfo })
            {
                process.Start();

                if (commands != null)
                {
                    process.OutputDataReceived += (s, e) => output += $"{e.Data}\n";
                    process.ErrorDataReceived += (s, e) => error += $"{e.Data}\n";

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    foreach (var command in commands)
                    {
                        process.StandardInput.WriteLine(command);
                    }
                }

                process.WaitForExit();
            }

            return (output, error);
        }
    }
}
