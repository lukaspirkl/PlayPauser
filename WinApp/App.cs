using HotKeyMapper;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PlayPauser
{
    [System.ComponentModel.DesignerCategory("")] // Disable Visual Studio form designer
    public partial class App : Form
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
            Options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "PlayPauser.json")));

            // https://stackoverflow.com/questions/49012233/winforms-4k-and-1080p-scaling-high-dpi
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);

            Application.Run(new App());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private KeyboardHook hook;
        private IWebHost webHost;
        private HttpClient httpClient = new HttpClient();
        //Form options = new OptionsForm();

        public static Options Options;

        public App()
        {
            webHost = new WebHostBuilder()
                .UseKestrel(o =>
                {
                    o.ListenAnyIP(Options.ServerPort);
                })
                .UseStartup<Startup>()
                .Build();
            webHost.RunAsync();

            hook = new KeyboardHook();
            hook.KeyPressed += async (s, e) =>
            {
                await WebSocketManager.Instance.Send("Key pressed");
                if (Options.IsHttpSender)
                {
                    try
                    {
                        await httpClient.PostAsync(Options.ClientAddress, new StringContent("aa"));
                    }
                    catch (Exception)
                    {
                        // TODO: Maybe dialog with error.
                    }
                }
            };
            hook.RegisterHotKey(HotKeyMapper.ModifierKeys.Alt, Keys.X);

            trayMenu = new ContextMenu();
            //trayMenu.MenuItems.Add("Options", (s, e) => options.Show());
            trayMenu.MenuItems.Add("Exit", (s, e) => Application.Exit());

            trayIcon = new NotifyIcon();
            trayIcon.Text = "PlayPauser";
            trayIcon.Icon = new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.icon.ico"));

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
                hook.Dispose();
                webHost.StopAsync().Wait(); // TODO: Integrate the web host somehow nicely.
            }

            base.Dispose(isDisposing);
        }
    }
}
