// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyDrawConditionDatabase
    {
        internal static AttributeToElementMap<PropertyDrawConditionAttribute, PropertyDrawCondition> mapper;

        static PropertyDrawConditionDatabase()
        {
            mapper = new AttributeToElementMap<PropertyDrawConditionAttribute, PropertyDrawCondition>();
            mapper.Update();
        }

        public static PropertyDrawCondition GetDrawConditionForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

