using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var profiles = Provide();

            mce.AddProfiles(profiles);

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            var mapper = new Mapper(mc, t => _container.GetInstance(t));

            return mapper;
        }

        internal IEnumerable<Profile> Provide()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var assembly = typeof(MapperProvider).Assembly;
            var assemblies = new List<Assembly> { assembly };

            foreach (var referencedAssemblyName in assembly
                .GetReferencedAssemblies()
                .Where(a => a.FullName.StartsWith("SurveyApp", StringComparison.OrdinalIgnoreCase)))
            {
                var referencedAssembly = loadedAssemblies.Find(l => l.FullName == referencedAssemblyName.FullName)
                    ?? AppDomain.CurrentDomain.Load(referencedAssemblyName);
                if (referencedAssembly.GetTypes().Any(t => typeof(Profile).IsAssignableFrom(t))
                    && !assemblies.Contains(referencedAssembly))
                {
                    assemblies.Add(referencedAssembly);
                }
            }

            var profileTypes = assemblies.SelectMany(ay => ay.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t))).Select(t => t);
            return CreateProfiles(profileTypes);
        }

        internal IEnumerable<Profile> CreateProfiles(IEnumerable<Type> types)
        {
            if (types == null)
            {
                yield break;
            }

            foreach (var type in types)
            {
                yield return Activator.CreateInstance(type) as Profile;
            }
        }
    }
}
