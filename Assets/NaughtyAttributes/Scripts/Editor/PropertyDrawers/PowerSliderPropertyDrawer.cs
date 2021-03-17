using Unity.Mathematics;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(PowerSlideAttribute))]
    public class PowerSliderPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            var attr = PropertyUtility.GetAttribute<PowerSlideAttribute>(property);
            if (attr.BaseRcpLn == 0 | attr.BaseRcpLn == float.NaN)
            {
                EditorDrawUtility.DrawHelpBox($"Invalid Base: {attr.Base}", MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
                EditorDrawUtility.DrawPropertyField(property);
                return;
            }

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                float value = math.log(property.intValue) * attr.BaseRcpLn;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PrefixLabel($"{property.displayName}[{property.intValue}]");
                value = EditorGUILayout.Slider(value, attr.MinPower, attr.MaxPower);
                property.intValue = (int)math.round(math.pow(attr.Base, value));
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float value = math.log(property.floatValue) * attr.BaseRcpLn;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PrefixLabel($"{property.displayName}[{property.floatValue}]");
                value = EditorGUILayout.Slider(value, attr.MinPower, attr.MaxPower);
                property.floatValue = math.pow(attr.Base, value);
            }
            else
            {
                string warning = attr.GetType().Name + " can be used only on int or float fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
                EditorDrawUtility.DrawPropertyField(property);
            }
        }
    }
}
