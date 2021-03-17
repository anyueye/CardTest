// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyMetaDatabase
    {
        internal static AttributeToElementMap<PropertyMetaAttribute, PropertyMeta> mapper;

        static PropertyMetaDatabase()
        {
            mapper = new AttributeToElementMap<PropertyMetaAttribute, PropertyMeta>();
            mapper.Update();
        }

        public static PropertyMeta GetMetaForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

