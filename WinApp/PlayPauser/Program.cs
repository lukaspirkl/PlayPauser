using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PlayPauser
{
    public static class Program
    {
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }


        [STAThread]
        public static void Main()
        {
            // https://stackoverflow.com/questions/49012233/winforms-4k-and-1080p-scaling-high-dpi
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);

            Application.Run(new App());
        }
    }
}
