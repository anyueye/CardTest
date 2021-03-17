// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyGrouperDatabase
    {
        internal static AttributeToElementMap<PropertyGrouperAttribute, PropertyGrouper> mapper;

        static PropertyGrouperDatabase()
        {
            mapper = new AttributeToElementMap<PropertyGrouperAttribute, PropertyGrouper>();
            mapper.Update();
        }

        public static PropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

