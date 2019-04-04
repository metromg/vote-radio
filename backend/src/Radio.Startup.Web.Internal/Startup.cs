using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radio.Infrastructure.Api.Internal;

namespace Radio.Startup.Web.Internal
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRadio();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IContainer rootContainer)
        {
            app.UseRadio(env, rootContainer);
        }
    }
}
