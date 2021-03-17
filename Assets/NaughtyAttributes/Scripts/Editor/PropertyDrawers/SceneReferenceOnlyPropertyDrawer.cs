using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(SceneReferenceOnlyAttribute))]
    public class SceneReferenceOnlyPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            var attr = PropertyUtility.GetAttribute<SceneReferenceOnlyAttribute>(property);
            var type = this.FieldInfo.FieldType;
            var target = property.serializedObject.targetObject;
            if (target is Component)
            {
                var targetGo = (target as Component).gameObject;
                if (typeof(GameObject).IsAssignableFrom(type))
                {
                    var obj = (GameObject)EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, this.FieldInfo.FieldType, true);
                    if (obj == null || obj.scene == targetGo.scene) property.objectReferenceValue = obj;
                    else Debug.LogWarning($"{obj.name} is not in the same scene[{targetGo.scene.name}] with {targetGo.name}", obj);
                    return;
                }
                else if (typeof(Component).IsAssignableFrom(type))
                {
                    var obj = (Component)EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, this.FieldInfo.FieldType, true);
                    if (obj == null || obj.gameObject.scene == targetGo.scene) property.objectReferenceValue = obj;
                    else Debug.LogWarning($"{obj.name} is not in the same scene[{targetGo.scene.name}] with {targetGo.name}", obj);
                    return;
                }
            }
            EditorDrawUtility.DrawHelpBox($"Field={this.FieldInfo.Name} Type={this.FieldInfo.FieldType.Name} {nameof(SceneReferenceOnlyAttribute)} can only be used on GameObject or Component fields of UnityEngine.Component", MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
            EditorDrawUtility.DrawPropertyField(property);
        }
    }
}
