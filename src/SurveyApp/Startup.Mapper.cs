using AutoMapper;
using SimpleInjector;
using SurveyApp.Mapping;

namespace SurveyApp
{
#pragma warning disable CA1822 // Mark members as static
    public partial class Startup
    {
        public void ConfigureMapping()
        {
            _container.RegisterSingleton(() => GetMapper(_container));
        }

        private static IMapper GetMapper(Container container)
        {
            var mapperProvider = container.GetInstance<MapperProvider>();
            var mapper = mapperProvider.GetMapper();
            return mapper;
        }
    }
#pragma warning restore CA1822 // Mark members as static
}
