using Microsoft.AspNetCore.Http;
using PlayPauser.Messages;
using System.Threading.Tasks;

namespace PlayPauser.Parts.AspNet
{
    public class RequestHandler
    {
        private readonly RequestDelegate next;

        public RequestHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, EventAggregator eventAggregator)
        {
            if (context.Request.Method == "POST")
            {
                eventAggregator.Publish(new HttpPostReceived());
            }
            else
            {
                await next(context);
            }
        }
    }
}
