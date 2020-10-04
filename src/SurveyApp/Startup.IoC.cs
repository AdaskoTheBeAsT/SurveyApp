using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SurveyApp
{
    public partial class Startup
    {
        public void ConfigureServicesIoC(IServiceCollection services)
        {
            services.AddSimpleInjector(
                _container,
                options =>
                {
                    // AddAspNetCore() wraps web requests in a Simple Injector scope.
                    options.AddAspNetCore()

                        // Ensure activation of a specific framework type to be created by
                        // Simple Injector instead of the built-in configuration system.
                        .AddControllerActivation();
                    options.AddLogging();
                });
        }

        public void ConfigureIoC(IApplicationBuilder app)
        {
            // UseSimpleInjector() enables framework services to be injected into
            // application components, resolved by Simple Injector.
            app.UseSimpleInjector(_container);
        }
    }
}
