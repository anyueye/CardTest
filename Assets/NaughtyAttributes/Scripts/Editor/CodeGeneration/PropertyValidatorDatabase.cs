// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyValidatorDatabase
    {
        internal static AttributeToElementMap<PropertyValidatorAttribute, PropertyValidator> mapper;

        static PropertyValidatorDatabase()
        {
            mapper = new AttributeToElementMap<PropertyValidatorAttribute, PropertyValidator>();
            mapper.Update();
        }

        public static PropertyValidator GetValidatorForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

