using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(TranformChildOnlyAttribute))]
    public class TranformChildOnlyPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            var attr = PropertyUtility.GetAttribute<PrefabOnlyAttribute>(property);
            var type = this.FieldInfo.FieldType;
            var target = property.serializedObject.targetObject;
            if (target is Component)
            {
                var parent = (target as Component).transform;
                if (typeof(GameObject).IsAssignableFrom(type))
                {
                    var obj = (GameObject)EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, this.FieldInfo.FieldType, true);
                    if (obj == null || obj.transform.IsChildOf(parent)) property.objectReferenceValue = obj;
                    else Debug.LogWarning($"{obj.name} is not child of {parent.name} thus can not be used", obj);
                    return;
                }
                else if (typeof(Component).IsAssignableFrom(type))
                {
                    var obj = (Component)EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, this.FieldInfo.FieldType, true);
                    if (obj == null || obj.transform.IsChildOf(parent)) property.objectReferenceValue = obj;
                    else Debug.LogWarning($"{obj.name} is not child of {parent.name} thus can not be used", obj);
                    return;
                }
            }
            EditorDrawUtility.DrawHelpBox($"Field={this.FieldInfo.Name} Type={this.FieldInfo.FieldType.Name} {nameof(TranformChildOnlyAttribute)} can only be used on GameObject or Component fields of UnityEngine.Component", MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
            EditorDrawUtility.DrawPropertyField(property);
        }
    }
}
