using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PlayPauser.Parts.AspNet
{
    public class AspNetStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<WebSocketManager>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();

            app.UseMiddleware<WebSocketHandler>();

            if (App.Options.IsHttpReceiver)
            {
                app.UseMiddleware<RequestHandler>();
            }
        }
    }
}
