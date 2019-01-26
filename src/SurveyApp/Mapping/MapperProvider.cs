using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;

namespace SurveyApp.Mapping
{
    public sealed class MapperProvider
    {
        private readonly Container _container;

        public MapperProvider(Container container)
        {
            _container = container;
        }

        public IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(_container.GetInstance);

            var profiles = typeof(MappingProfile).Assembly.GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t))
                .ToList();

            mce.AddProfiles(profiles);

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            var mapper = new Mapper(mc, t => _container.GetInstance(t));

            return mapper;
        }
    }
}
