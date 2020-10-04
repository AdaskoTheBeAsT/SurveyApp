using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Serilog;
using SimpleInjector;

namespace SurveyApp
{
    public sealed partial class Startup
        : IDisposable
    {
        private readonly Container _container = new Container();

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
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(
                    options =>
                    {
                        // https://security-code-scan.github.io/#SCS0028
                        // implemented as white list
#pragma warning disable SCS0028
#pragma warning disable SEC0030
                        options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
#pragma warning restore SEC0030
#pragma warning restore SCS0028
                        options.SerializerSettings.SerializationBinder = new LimitedBinder();
                        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    });

            ConfigureServicesIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureIoC(app);
            ConfigureMapping();
            _container.Verify();

            ConfigureSwagger(app);
            app.UseDefaultFiles();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        if (context.File.Name.Equals("index.html", StringComparison.OrdinalIgnoreCase))
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

            app.UseRouting();
            ConfigureCors(app);

            app.UseEndpoints(
                endpoints => endpoints.MapDefaultControllerRoute());

            Log.Information("Started");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container?.Dispose();
            }
        }
    }
}
