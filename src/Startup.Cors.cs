using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace SurveyApp
{
    #pragma warning disable CA1822 // Mark members as static
    public partial class Startup
    {
        public static readonly string AllowDev = "AllowDev";

        public void ConfigureServicesCors(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(
                    AllowDev,
                    p =>
                        p
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()));

            services.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory(AllowDev)));
        }

        public void ConfigureCors(IApplicationBuilder app)
        {
            app.UseCors(AllowDev);
        }
    }
    #pragma warning restore CA1822 // Mark members as static
}