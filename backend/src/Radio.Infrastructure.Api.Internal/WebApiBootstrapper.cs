using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Radio.Core;
using Radio.Infrastructure.Api.Pipeline;

namespace Radio.Infrastructure.Api.Internal
{
    public static class WebApiBootstrapper
    {
        public static void AddRadio(this IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().AddJsonOptions(ConfigureJsonSerializer);

            // Add custom controller activator
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, UnitOfWorkControllerActivator>());
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
        }

        private static void ConfigureJsonSerializer(MvcJsonOptions options)
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        private static void ConfigureCorsUsage(CorsPolicyBuilder builder)
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    }
}
