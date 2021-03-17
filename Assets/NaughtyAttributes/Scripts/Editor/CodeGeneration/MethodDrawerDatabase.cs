// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class MethodDrawerDatabase
    {
        internal static AttributeToElementMap<MethodDrawerAttribute, MethodDrawer> mapper;

        static MethodDrawerDatabase()
        {
            mapper = new AttributeToElementMap<MethodDrawerAttribute, MethodDrawer>();
            mapper.Update();
        }

        public static MethodDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

