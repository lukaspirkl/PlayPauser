using PlayPauser.Messages;
using PlayPauser.Parts.Hook;
using System.Windows.Forms;

namespace PlayPauser.Parts
{
    public class KeyboardHookPart : IPart
    {
        private readonly EventAggregator eventAggregator;
        private KeyboardHook hook;

        public KeyboardHookPart(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Start(Options options)
        {
            hook = new KeyboardHook();
            hook.KeyPressed += (s, e) =>
            {
                eventAggregator.Publish(new KeyPressed());
            };
            hook.RegisterHotKey(ModifierKeys.Alt, Keys.X);
        }

        public void Stop()
        {
            hook.Dispose();
            hook = null;
        }
    }
}
