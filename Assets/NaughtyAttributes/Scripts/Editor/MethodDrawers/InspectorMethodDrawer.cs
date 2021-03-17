using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NaughtyAttributes;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;



namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(InspectorDrawerAttribute))]
    public class InspectorMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                InspectorDrawerAttribute buttonAttribute = (InspectorDrawerAttribute)methodInfo.GetCustomAttributes(typeof(InspectorDrawerAttribute), true)[0];

                methodInfo.Invoke(target, null);
            }
            else
            {
                string warning = typeof(InspectorDrawerAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }

        }
    }
}