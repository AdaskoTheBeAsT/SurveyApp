using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SurveyApp
{
    public partial class Startup
    {
        public AppLogConfiguration LogConfiguration { get; set; }

        public void StartupLogging(IHostingEnvironment env)
        {
            // _container.Options.DependencyInjectionBehavior =
            //    new SerilogContextualLoggerInjectionBehavior(_container.Options);
            var lastPath = Configuration.GetSection("Serilog:WriteTo:0:Args:pathFormat").Get<string>();
            if (string.IsNullOrEmpty(lastPath))
            {
                lastPath = "/logs";
            }

            var path = Path.GetDirectoryName(Path.Combine(env.ContentRootPath, lastPath));

            LogConfiguration = new AppLogConfiguration
            {
                Path = path,
            };
        }

        public void ConfigureLoggingAfterIoC()
        {
            _container.RegisterInstance(LogConfiguration);
        }

#pragma warning disable CA1034
        public class AppLogConfiguration
        {
            public string Path { get; set; }
        }
#pragma warning restore CA1034
    }
}
