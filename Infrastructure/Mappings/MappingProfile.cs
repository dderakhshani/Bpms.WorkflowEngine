using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Bpms.WorkflowEngine.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(AppDomain.CurrentDomain.GetAssemblies());
        }

        private void ApplyMappingsFromAssembly(Assembly[] assemblies)
        {
            var mappingTypes = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in mappingTypes)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}