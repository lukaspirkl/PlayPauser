using Fleck;
using HotKeyMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PlayPauser
{
    [System.ComponentModel.DesignerCategory("")] // Disable Visual Studio form designer
    public partial class App : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new App());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private KeyboardHook hook;
        private WebSocketServer server;
        private List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();

        public App()
        {
            server = new WebSocketServer("ws://127.0.0.1:8181")
            {
                RestartAfterListenError = true
            };
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                //socket.OnMessage = message => socket.Send(message);
            });


            hook = new KeyboardHook();
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            hook.RegisterHotKey(HotKeyMapper.ModifierKeys.Alt, Keys.X);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "PlayPauser";
            trayIcon.Icon = new Icon(GetType().Assembly.GetManifestResourceStream("PlayPauser.icon.ico"));

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            foreach (var socket in allSockets)
            {
                socket.Send("Key pressed");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
                hook.Dispose();
                server.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
