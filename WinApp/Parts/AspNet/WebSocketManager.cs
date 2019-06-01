using PlayPauser.Messages;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlayPauser.Parts.AspNet
{
    public class WebSocketManager : IDisposable
    {
        private readonly ConcurrentDictionary<WebSocket, object> webSockets = new ConcurrentDictionary<WebSocket, object>();
        private readonly EventAggregator eventAggregator;

        public WebSocketManager(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe<KeyPressed>(OnKeyPressed);
            eventAggregator.Subscribe<HttpPostReceived>(OnHttpPostReceived);
        }

        private void OnKeyPressed(KeyPressed message)
        {
            Send("Key pressed").Wait(); //TODO: Somehow remove Wait()
        }

        private void OnHttpPostReceived(HttpPostReceived message)
        {
            Send("Post received").Wait(); //TODO: Somehow remove Wait()
        }

        public void Add(WebSocket ws)
        {
            webSockets.TryAdd(ws, null); // TODO: Handle failure
        }

        public void Dispose()
        {
            eventAggregator.Unsubscribe<KeyPressed>(OnKeyPressed);
            eventAggregator.Unsubscribe<HttpPostReceived>(OnHttpPostReceived);
        }

        public void Remove(WebSocket ws)
        {
            webSockets.TryRemove(ws, out var o); // TODO: Handle failure
        }

        public async Task Send(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            foreach (var ws in webSockets.Keys)
            {
                await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
