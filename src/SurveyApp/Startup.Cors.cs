using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace SurveyApp
{
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
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(_ => true)
                            .AllowCredentials()));

            services.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory(AllowDev)));
        }

        public void ConfigureCors(IApplicationBuilder app)
        {
            app.UseCors(AllowDev);
        }
    }
}