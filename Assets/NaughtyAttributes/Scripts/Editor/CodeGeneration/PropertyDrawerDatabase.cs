// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyDrawerDatabase
    {
        internal static AttributeToElementMap<PropertyDrawerAttribute, PropertyDrawer> mapper;

        static PropertyDrawerDatabase()
        {
            mapper = new AttributeToElementMap<PropertyDrawerAttribute, PropertyDrawer>();
            mapper.Update();
        }

        public static PropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }

        public static void ClearCache()
        {
            foreach (var kvp in mapper.elemByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}

