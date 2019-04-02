using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Radio.Core;
using Radio.Infrastructure.Api.External.Hubs;
using Radio.Infrastructure.Api.External.Pipeline;
using Radio.Infrastructure.Api.Pipeline;

namespace Radio.Infrastructure.Api.External
{
    public static class WebApiBootstrapper
    {
        public static void AddRadio(this IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().AddJsonOptions(ConfigureJsonSerializer);
            services.AddSignalR();

            // Add custom controller & hub activator
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, UnitOfWorkControllerActivator>());
            services.Replace(ServiceDescriptor.Transient<IHubActivator<RadioHub>>(p => new AutofacContainerHubActivator<RadioHub>(p)));
        }

        public static void UseRadio(this IApplicationBuilder app, IHostingEnvironment env, IContainer container)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseScopeMiddleware(container.Resolve<IUnitOfWorkFactory<ILifetimeScope>>());
            app.UseGlobalExceptionHandler();
            app.UseCors(ConfigureCorsUsage);

            app.UseMvc();
            app.UseSignalR(ConfigureSignalRUsage);
        }

        private static void ConfigureJsonSerializer(MvcJsonOptions options)
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        private static void ConfigureCorsUsage(CorsPolicyBuilder builder)
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }

        private static void ConfigureSignalRUsage(HubRouteBuilder routeBuilder)
        {
            routeBuilder.MapHub<RadioHub>("/radioHub");
        }
    }
}
