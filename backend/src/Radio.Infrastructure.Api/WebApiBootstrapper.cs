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
using Radio.Infrastructure.Api.Pipeline;

namespace Radio.Infrastructure.Api
{
    public static class WebApiBootstrapper
    {
        public static void AddRadioBase(this IServiceCollection services)
        {
            services.AddCors();
        }

        public static void AddRadioMvc(this IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(ConfigureJsonSerializer);

            // Add custom controller activator
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, UnitOfWorkControllerActivator>());
        }

        public static void AddRadioSignalR(this IServiceCollection services)
        {
            services.AddSignalR();

            // Add custom hub activator
            //services.Replace(ServiceDescriptor.Transient<IHubActivator<RadioChangeHub>>(p => new AutofacContainerHubActivator<RadioChangeHub>(p)));
        }

        public static void UseRadioBase(this IApplicationBuilder app, IHostingEnvironment env, IContainer container)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseScopeMiddleware(container.Resolve<IUnitOfWorkFactory<ILifetimeScope>>());
            app.UseGlobalExceptionHandler();
            app.UseCors(ConfigureCorsUsage);
        }

        public static void UseRadioMvc(this IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public static void UseRadioSignalR(this IApplicationBuilder app)
        {
            app.UseSignalR(ConfigureSignalRUsage);
        }

        private static void ConfigureJsonSerializer(MvcJsonOptions options)
        {
            options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        private static void ConfigureCorsUsage(CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }

        private static void ConfigureSignalRUsage(HubRouteBuilder routeBuilder)
        {
            //routeBuilder.MapHub<RadioChangeHub>("/radioChange");
        }
    }
}
