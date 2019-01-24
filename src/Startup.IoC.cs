using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace SurveyApp
{
#pragma warning disable CA1822 // Mark members as static
    public partial class Startup
    {
        public void ConfigureServicesIoC(IServiceCollection services)
        {
            // Simple injector part for proper controller activation
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(_container));
            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        public void ConfigureIoC(IApplicationBuilder app)
        {
            _container.RegisterMvcControllers(app);
            _container.AutoCrossWireAspNetComponents(app);
        }
    }
#pragma warning restore CA1822 // Mark members as static
}
