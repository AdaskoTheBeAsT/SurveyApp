using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

namespace SurveyApp
{
    public partial class Startup
    {
        private const string SwaggerEndpoint = "/swagger/v1/swagger.json";
        private const string RedocEndpoint = "../swagger/v1/swagger.json";
        private const string RedocRoutePrefix = "api-docs";
        private const string ExtensionDll = ".dll";
        private const string ExtensionExe = ".exe";
        private const string ExtensionXml = ".xml";
        private const string V1 = "v1";
        private const string DotStartup = ".Startup";

        public void ConfigureServicesSwagger(IServiceCollection services)
        {
            // both services for swagger
            services.TryAddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(V1, new OpenApiInfo { Title = "EPCalculator API", Version = V1 });

                // Set the comments path for the swagger json and ui.
                // based on http://michaco.net/blog/TipsForUsingSwaggerAndAutorestInAspNetCoreMvcServices
                var fileName = GetType().GetTypeInfo().Module.Name
                    .Replace(ExtensionDll, ExtensionXml, StringComparison.OrdinalIgnoreCase)
                    .Replace(ExtensionExe, ExtensionXml, StringComparison.OrdinalIgnoreCase);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, fileName));
            });
        }

        public void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
                SwaggerEndpoint,
                $"{GetType().FullName?.Replace(DotStartup, string.Empty, StringComparison.OrdinalIgnoreCase)}"));
            app.UseReDoc(c =>
            {
                c.SpecUrl = RedocEndpoint;
                c.RoutePrefix = RedocRoutePrefix;
            });
        }
    }
}
