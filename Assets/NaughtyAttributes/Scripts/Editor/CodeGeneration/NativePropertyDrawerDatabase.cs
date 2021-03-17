// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class NativePropertyDrawerDatabase
    {
        internal static AttributeToElementMap<NativePropertyDrawerAttribute, NativePropertyDrawer> mapper;

        static NativePropertyDrawerDatabase()
        {
            mapper = new AttributeToElementMap<NativePropertyDrawerAttribute, NativePropertyDrawer>();
            mapper.Update();
        }

        public static NativePropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

