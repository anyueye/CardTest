using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ReorderableListAttribute))]
    public class ReorderableListPropertyDrawer : PropertyDrawer
    {
        private static Dictionary<string, ReorderableList> listViewCache = new Dictionary<string, ReorderableList>();

        static Regex arrayElementMatch = new Regex("\\.Array\\.data\\[\\d+\\]");
        string GetPropertyKeyName(SerializedProperty property) { return arrayElementMatch.Replace(property.propertyPath, ""); }

        ReorderableList GetListView(SerializedProperty property)
        {
            string key = GetPropertyKeyName(property);
            if (!listViewCache.TryGetValue(key, out var listView))
            {
                listView = new ReorderableList(property.serializedObject, property, true, true, true, true);
                listViewCache.Add(key, listView);
            }
            listView.serializedProperty = property;
            listView.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, $"{property.displayName}: [{property.arraySize}]", EditorStyles.label);
            };

            listView.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = property.GetArrayElementAtIndex(index);
                rect.y += 1.0f;
                rect.x += 10.0f;
                rect.width -= 10.0f;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 0.0f), element, true);
            };
            listView.elementHeightCallback = (int index) => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;

            listView.onCanRemoveCallback = (ReorderableList list) => property.arraySize > 0;

            listView.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                int sizeBeforeDelete = property.arraySize;
                int maxIdx = sizeBeforeDelete - 1;
                index = index < 0 ? maxIdx : index;
                index = index > maxIdx ? maxIdx : index;
                property.DeleteArrayElementAtIndex(index);
                int sizeAfterDelete = property.arraySize;
                if (sizeAfterDelete > 0 && sizeAfterDelete == sizeBeforeDelete)
                {
                    //UnityEngine.Object Reference deleted, still need to delete null array element
                    property.DeleteArrayElementAtIndex(index);
                }
                maxIdx--;
                list.index = Mathf.Min(maxIdx, index);
            };
            listView.onAddCallback = (ReorderableList list) =>
            {
                int count = property.arraySize;
                int index = list.index;
                if (index < 0 || index > count) index = count;
                property.InsertArrayElementAtIndex(index);
                list.index = index;
            };
            return listView;
        }

        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            if (property.isArray)
            {
                var listView = GetListView(property);
                listView.DoLayoutList();
                if (listView.HasKeyboardControl())
                {
                    if (Event.current.type == EventType.KeyDown)
                    {
                        if (Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.KeypadMinus)
                        {
                            if (listView.onCanRemoveCallback(listView)) listView.onRemoveCallback(listView);
                        }
                        else if (Event.current.keyCode == KeyCode.KeypadPlus)
                        {
                            listView.onAddCallback(listView);
                        }
                    }
                }
            }
            else
            {
                string warning = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property));

                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        public override void OnClearCache()
        {
            listViewCache.Clear();
        }
    }
}
