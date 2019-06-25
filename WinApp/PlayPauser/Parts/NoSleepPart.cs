using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PlayPauser.Parts
{
    public class NoSleepPart : IPart
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [Flags]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }

        private string fileName;
        private Process p;

        public void Start(Options options)
        {
            fileName = Path.Combine(Path.GetTempPath(), "NoSleep.exe");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            WriteResourceToFile(fileName);

            p = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        public void Stop()
        {
            p.Kill();
            p.WaitForExit();

            try
            {
                File.Delete(fileName);
            }
            catch { } // best effort

            fileName = null;
        }

        private void WriteResourceToFile(string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("NoSleep.exe"))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }
    }
}
