using System;
using System.Collections.Generic;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;

namespace Persistity.Registries
{
    public class MappingRegistry : IMappingRegistry
    {
        public ITypeMapper TypeMapper { get; private set; }
        public IDictionary<Type, TypeMapping> TypeMappings { get; private set; }

        public MappingRegistry(ITypeMapper typeMapper)
        {
            TypeMapper = typeMapper;
            TypeMappings = new Dictionary<Type, TypeMapping>();
        }

        public TypeMapping GetMappingFor<T>() where T : new()
        {
            var type = typeof(T);
            return GetMappingFor(type);
        }

        public TypeMapping GetMappingFor(Type type)
        {
            if(TypeMappings.ContainsKey(type))
            { return TypeMappings[type]; }

            var typeMapping = TypeMapper.GetTypeMappingsFor(type);
            TypeMappings.Add(type, typeMapping);
            return typeMapping;
        }
    }
}