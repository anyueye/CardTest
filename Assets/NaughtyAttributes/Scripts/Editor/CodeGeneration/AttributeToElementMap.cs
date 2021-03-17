// This class is auto generated

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace NaughtyAttributes.Editor
{
    public class AttributeToElementMap<TAttribute, TElem>
        where TAttribute : BaseAttribute
        where TElem : class
    {
        public Dictionary<Type, TElem> elemByAttributeType = new Dictionary<Type, TElem>();

        public void Update()
        {
            elemByAttributeType.Clear();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(TElem)))
                    {
                        var attList = t.GetCustomAttributes(typeof(TAttribute), true);
                        for (int i = 0; i < attList.Length; i++)
                        {
                            var attr = attList[i];
                            var targetAttrType = (attr as TAttribute).TargetAttributeType;
                            if (elemByAttributeType.ContainsKey(targetAttrType))
                                UnityEngine.Debug.LogError($"{typeof(TAttribute).Name}({targetAttrType.Name}) already register with [{t.Name}]");
                            else elemByAttributeType[targetAttrType] = Activator.CreateInstance(t) as TElem;

                        }
                    }
                }
            }
        }
    }
}


