using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SimpleInjector;
using SurveyApp.Middleware;

namespace SurveyApp
{
    public sealed partial class Startup
        : IDisposable
    {
        private Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesSwagger(services);
            ConfigureServicesCors(services);

            services
                .AddWebApi()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureJson();

            ConfigureServicesIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureIoC(app);
            ConfigureMapping();
            _container.Verify();

            ConfigureSwagger(app);
            ConfigureCors(app);
            app.UseDefaultFiles();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        if (context.File.Name == "index.html")
                        {
                            context.Context.Response.Headers.Append(HeaderNames.CacheControl, "no-cache,no-store");
                            context.Context.Response.Headers.Append(HeaderNames.Pragma, "no-cache");
                            context.Context.Response.Headers.Append(HeaderNames.Expires, "-1");
                        }
                        else
                        {
                            context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=86400";
                        }
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
}
