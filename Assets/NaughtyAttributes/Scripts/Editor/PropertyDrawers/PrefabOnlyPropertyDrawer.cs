using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    //PrefabOnlyAttribute
    //TranformChildOnlyAttribute
    [PropertyDrawer(typeof(PrefabOnlyAttribute))]
    public class PrefabOnlyPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            var attr = PropertyUtility.GetAttribute<PrefabOnlyAttribute>(property);
            var type = this.FieldInfo.FieldType;
            if (typeof(GameObject).IsAssignableFrom(type) || typeof(Component).IsAssignableFrom(type))
            {
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, this.FieldInfo.FieldType, false);
            }
            else
            {
                EditorDrawUtility.DrawHelpBox($"Field={this.FieldInfo.Name} Type={this.FieldInfo.FieldType.Name} {nameof(PrefabOnlyAttribute)} can only be used on GameObject or Component fields", MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
                EditorDrawUtility.DrawPropertyField(property);
            }
        }
    }
}
