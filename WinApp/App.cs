using Newtonsoft.Json;
using PlayPauser.Messages;
using PlayPauser.Parts;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PlayPauser
{
    [System.ComponentModel.DesignerCategory("")] // Disable Visual Studio form designer
    public partial class App : Form
    {
        private IPart[] parts;

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        //Form options = new OptionsForm();

        public static Options Options;

        public App()
        {
            var eventAggregator = new EventAggregator();

            parts = new IPart[]
            {
                new AspNetPart(eventAggregator),
                new HttpSendPart(eventAggregator),
                new KeyboardHookPart(eventAggregator),
                new NoSleepPart()
            };

            Options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "PlayPauser.json")));
            foreach (var part in parts)
            {
                part.Start(Options);
            }

            var whiteIcon = new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.White.ico"));
            var blueIcon = new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.Blue.ico"));
            var greenIcon = new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.Green.ico"));

            trayMenu = new ContextMenu();
            //trayMenu.MenuItems.Add("Options", (s, e) => options.Show());
            trayMenu.MenuItems.Add("Exit", (s, e) => Application.Exit());

            trayIcon = new NotifyIcon();
            trayIcon.Text = "PlayPauser";
            trayIcon.Icon = whiteIcon;

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (s, a) =>
            {
                trayIcon.Icon = whiteIcon;
                timer.Stop();
            };

            eventAggregator.Subscribe<KeyPressed>(m =>
            {
                trayIcon.Icon = greenIcon;
                timer.Start();
            });

            eventAggregator.Subscribe<HttpPostReceived>(m =>
            {
                trayIcon.Icon = blueIcon;
                timer.Start();
            });

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
                foreach (var part in parts)
                {
                    part.Stop();
                }
            }

            base.Dispose(isDisposing);
        }
    }
}
