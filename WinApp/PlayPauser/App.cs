using Newtonsoft.Json;
using PlayPauser.Messages;
using PlayPauser.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PlayPauser
{
    [System.ComponentModel.DesignerCategory("")] // Disable Visual Studio form designer
    public partial class App : Form
    {
        private delegate void SafeCallDelegate(IconColor iconColor);

        private enum IconColor
        {
            White,
            Blue,
            Green
        }

        private Dictionary<IconColor, Icon> icons;

        Timer iconTimer = new Timer();

        private IPart[] parts;

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        //Form options = new OptionsForm();

        public static Options Options;

        public App()
        {
            var eventAggregator = new EventAggregator();

            icons = new Dictionary<IconColor, Icon>
            {
                { IconColor.White, new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.White.ico")) },
                { IconColor.Green, new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.Green.ico")) },
                { IconColor.Blue, new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.Icons.Blue.ico")) },
            };

            parts = new IPart[]
            {
                new NoSleepPart(),
                new AspNetPart(eventAggregator),
                new HttpSendPart(eventAggregator),
                new KeyboardHookPart(eventAggregator)
            };

            Options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location), "PlayPauser.json")));
            foreach (var part in parts)
            {
                part.Start(Options);
            }

            trayMenu = new ContextMenu();
            //trayMenu.MenuItems.Add("Options", (s, e) => options.Show());
            trayMenu.MenuItems.Add("Exit", (s, e) => Application.Exit());

            trayIcon = new NotifyIcon();
            trayIcon.Text = "PlayPauser";

            SetIcon(IconColor.White);

            
            iconTimer.Interval = 500;
            iconTimer.Tick += (s, a) =>
            {
                trayIcon.Icon = icons[IconColor.White];
                iconTimer.Stop();
            };

            eventAggregator.Subscribe<KeyPressed>(m =>
            {
                Invoke(new SafeCallDelegate(SetIcon), IconColor.Green);
            });

            eventAggregator.Subscribe<HttpPostReceived>(m =>
            {
                Invoke(new SafeCallDelegate(SetIcon), IconColor.Blue);
            });

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void SetIcon(IconColor iconColor)
        {
            trayIcon.Icon = icons[iconColor];
            iconTimer.Start();
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
