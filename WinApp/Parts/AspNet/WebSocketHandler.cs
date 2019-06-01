using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PlayPauser.Parts.AspNet
{
    public class WebSocketHandler
    {
        private readonly RequestDelegate next;

        public WebSocketHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, WebSocketManager webSocketManager)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                webSocketManager.Add(webSocket);
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                webSocketManager.Remove(webSocket);
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            else
            {
                await next(context);
            }
        }
    }
}
