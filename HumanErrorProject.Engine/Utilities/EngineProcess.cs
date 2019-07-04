using System;
using System.Diagnostics;
using System.IO;
using HumanErrorProject.Engine.Data;

namespace HumanErrorProject.Engine.Utilities
{
    public class EngineProcess
    {
        protected Process Process;
        protected int WaitForExit;
        protected bool Read;

        public EngineProcess(EngineProcessData data)
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                    Arguments = $"-Command \"{data.Command} {data.Arguments}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    WorkingDirectory = data.WorkingDirectory
                }
            };
            WaitForExit = data.WaitForExit;
            Read = false;
        }

        public int Run()
        {
            Process.Start();
            if (!Process.WaitForExit(WaitForExit))
            {
                Process.Kill();
                return -1;
            }
            var exitCode = Process.ExitCode;
            return exitCode;
        }

        public void Stop()
        {
            if (!Read)
            {
                using (var stream = StandardError)
                {
                    stream.ReadToEnd();
                }
            }
            Process.Close();
            Process.Dispose();
        }

        public StreamReader StandardError
        {
            get
            {
                Read = true;
                return Process.StandardError;
            }
        }
    }
}
