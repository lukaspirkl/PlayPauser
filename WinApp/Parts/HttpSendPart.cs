using PlayPauser.Messages;
using System.Net.Http;

namespace PlayPauser.Parts
{
    public class HttpSendPart : IPart
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly EventAggregator eventAggregator;
        private Options options;

        public HttpSendPart(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Start(Options options)
        {
            this.options = options;
            if (options.IsHttpSender)
            {
                eventAggregator.Subscribe<KeyPressed>(OnKeyPressed);
            }
        }

        public void Stop()
        {
            eventAggregator.Unsubscribe<KeyPressed>(OnKeyPressed);
        }

        private void OnKeyPressed(KeyPressed message)
        {
            httpClient.PostAsync(options.ClientAddress, new StringContent("Key pressed"));
        }
    }
}
