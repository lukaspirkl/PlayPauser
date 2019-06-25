using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PlayPauser.Parts
{
    public class NoSleepPart : IPart
    {
        private bool enabled = true;
        private string fileName;
        private Process p;

        public void Start(Options options)
        {
            enabled = options.IsNoSleepEnabled;
            if (!enabled)
            {
                return;
            }

            fileName = Path.Combine(Path.GetTempPath(), "NoSleep.exe");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            WriteResourceToFile(fileName);

            p = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                WindowStyle = ProcessWindowStyle.Hidden,
            });
        }

        public void Stop()
        {
            if (!enabled)
            {
                return;
            }

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
