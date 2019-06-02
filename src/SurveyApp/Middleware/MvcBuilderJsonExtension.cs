using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SurveyApp.Middleware
{
    public static class MvcBuilderJsonExtension
    {
        public static void ConfigureJson(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                // https://security-code-scan.github.io/#SCS0028
                // implemented as white list
#pragma warning disable SCS0028
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
#pragma warning restore SCS0028
                options.SerializerSettings.SerializationBinder = new LimitedBinder();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    CamelCaseText = true,
                    AllowIntegerValues = true,
                });
            });
        }

#pragma warning disable CA1034
        [Serializable]
        public class TypeNotWhitelistedException
            : Exception
        {
            public TypeNotWhitelistedException()
                : base()
            {
            }

            public TypeNotWhitelistedException(string message)
                : base(message)
            {
            }

            public TypeNotWhitelistedException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            protected TypeNotWhitelistedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                : base(serializationInfo, streamingContext)
            {
            }
        }
#pragma warning restore CA1034

        private class LimitedBinder
            : ISerializationBinder
        {
            private readonly HashSet<Type> _allowedTypes = new HashSet<Type>
                {
                    typeof(Exception),
                    typeof(List<Exception>),
                };

            public Type BindToType(string assemblyName, string typeName)
            {
                var type = Type.GetType($"{typeName}, {assemblyName}", true);
                if (_allowedTypes.Contains(type))
                {
                    return type;
                }

                // Don’t return null for unexpected types –
                // this makes some serializers fall back to a default binder, allowing exploits.
                throw new TypeNotWhitelistedException("Unexpected serialized type");
            }

            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }
        }
    }
}