// This class is auto generated

using System;
using System.Diagnostics;

namespace NaughtyAttributes.Editor
{
    public static class FieldDrawerDatabase
    {
        internal static AttributeToElementMap<FieldDrawerAttribute, FieldDrawer> mapper;

        static FieldDrawerDatabase()
        {
            mapper = new AttributeToElementMap<FieldDrawerAttribute, FieldDrawer>();
            mapper.Update();
        }

        public static FieldDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (mapper.elemByAttributeType.TryGetValue(attributeType, out var drawer)) return drawer;
            else return null;
        }
    }
}

