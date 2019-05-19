using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlayPauser
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(WebSocketManager.Instance);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();

            app.UseMiddleware<WebSocketHandler>();

            if (App.Options.IsHttpReveiver)
            {
                app.UseMiddleware<RequestHandler>();
            }
        }
    }

    public class RequestHandler
    {
        private readonly RequestDelegate next;

        public RequestHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, WebSocketManager webSocketManager)
        {
            if (context.Request.Method == "POST")
            {
                await webSocketManager.Send("Post request received");
            }
            else
            {
                await next(context);
            }
        }
    }

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

    public class WebSocketManager
    {
        //TODO: Save this in IoC awailable to web host and winforms
        private static readonly WebSocketManager instance = new WebSocketManager();

        static WebSocketManager()
        {
        }

        private WebSocketManager()
        {
        }


        private readonly ConcurrentDictionary<WebSocket, object> webSockets = new ConcurrentDictionary<WebSocket, object>();

        public static WebSocketManager Instance => instance;

        public void Add(WebSocket ws)
        {
            webSockets.TryAdd(ws, null); // TODO: Handle failure
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
