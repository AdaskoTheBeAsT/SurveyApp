using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SurveyApp
{
#pragma warning disable CA1822 // Mark members as static
    public sealed partial class Startup
        : IDisposable
    {
        private Container _container;

        public Startup(
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            _container = new Container();
            Configuration = configuration;
            StartupLogging(env);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesIoC(services);
            ConfigureServicesSwagger(services);
            ConfigureServicesCors(services);

            services
                .AddWebApi()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureIoC(app);
            ConfigureLoggingAfterIoC();
            ConfigureMapping();
            _container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureSwagger(app);
            ConfigureCors(app);
            app.UseDefaultFiles();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
                        context.Context.Response.Headers.Append("Pragma", "no-cache");
                        context.Context.Response.Headers.Append("Expires", "-1");
                    },
                });
            app.UseMvc();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container?.Dispose();
                _container = null;
            }
        }
    }
#pragma warning restore CA1822 // Mark members as static
}
