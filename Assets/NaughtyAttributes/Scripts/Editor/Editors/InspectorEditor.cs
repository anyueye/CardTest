using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class InspectorEditor : UnityEditor.Editor
    {
        protected SerializedProperty script;

        protected IEnumerable<FieldInfo> fields;
        protected HashSet<FieldInfo> groupedFields;
        protected Dictionary<string, List<FieldInfo>> groupedFieldsByGroupName;
        protected Dictionary<string, bool> groupedState;


        protected IEnumerable<FieldInfo> nonSerializedFields;
        protected IEnumerable<PropertyInfo> nativeProperties;
        protected IEnumerable<MethodInfo> methods;

        protected Dictionary<string, SerializedProperty> serializedPropertiesByFieldName;

        protected bool useDefaultInspector;

        protected virtual void OnEnable()
        {
            try { this.script = this.serializedObject.FindProperty("m_Script"); }
            //catch (Exception) { return; }
            catch (Exception e) { Debug.LogException(e); return; }

            // Cache serialized fields
            this.fields = ReflectionUtility.GetAllFields(this.target, f => this.serializedObject.FindProperty(f.Name) != null);

            // If there are no NaughtyAttributes use default inspector
            if (this.fields.All(f => f.GetCustomAttributes(typeof(NaughtyAttribute), true).Length == 0)) this.useDefaultInspector = true;
            else
            {
                this.useDefaultInspector = false;
                // Cache grouped fields
                this.groupedFields = new HashSet<FieldInfo>(this.fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0));

                // Cache grouped fields by group name
                this.groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                foreach (var groupedField in this.groupedFields)
                {
                    string groupName = (groupedField.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name;
                    if (this.groupedFieldsByGroupName.ContainsKey(groupName)) this.groupedFieldsByGroupName[groupName].Add(groupedField);
                    else this.groupedFieldsByGroupName[groupName] = new List<FieldInfo>() { groupedField };
                }

                //
                groupedState = new Dictionary<string, bool>();
                UpdateGroupState();

                // Cache serialized properties by field name
                this.serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                foreach (var field in this.fields) this.serializedPropertiesByFieldName[field.Name] = this.serializedObject.FindProperty(field.Name);
            }

            // Cache non-serialized fields
            this.nonSerializedFields = ReflectionUtility.GetAllFields(
                this.target, f => f.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0 && this.serializedObject.FindProperty(f.Name) == null);

            // Cache the native properties
            this.nativeProperties = ReflectionUtility.GetAllProperties(
                this.target, p => p.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);

            // Cache methods with DrawerAttribute
            this.methods = ReflectionUtility.GetAllMethods(
                this.target, m => m.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);
        }

        protected virtual void OnDisable()
        {
            PropertyDrawerDatabase.ClearCache();
            this.fields = null;
            this.fields = null;
            this.groupedFields = null;
            this.groupedFieldsByGroupName = null;
            this.nonSerializedFields = null;
            this.nativeProperties = null;
            this.methods = null;
        }

        public override void OnInspectorGUI()
        {
            if (this.useDefaultInspector) this.DrawDefaultInspector();
            else
            {
                this.serializedObject.Update();

                if (this.script != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(this.script);
                    GUI.enabled = true;
                }

                // Draw fields
                HashSet<string> drawnGroups = new HashSet<string>();
                foreach (var field in this.fields)
                {
                    if (this.groupedFields.Contains(field))
                    {
                        // Draw grouped fields
                        string groupName = (field.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name;
                        if (!drawnGroups.Contains(groupName))
                        {
                            drawnGroups.Add(groupName);
                            if (groupedState.TryGetValue(groupName, out bool groupShow) && !groupShow) continue;

                            PropertyGrouper grouper = this.GetPropertyGrouperForField(field);
                            var groupFields = this.groupedFieldsByGroupName[groupName];
                            bool shouldDrawGroup = false;
                            foreach (var f in groupFields) shouldDrawGroup |= ShouldDrawField(f);
                            if (shouldDrawGroup)
                            {
                                if (grouper != null)
                                {
                                    grouper.BeginGroup(groupName);
                                    this.ValidateAndDrawFields(groupFields);
                                    grouper.EndGroup();
                                }
                                else this.ValidateAndDrawFields(groupFields);
                            }
                        }
                    }
                    else this.ValidateAndDrawField(field);// Draw non-grouped field
                }


                if (this.serializedObject.ApplyModifiedProperties()) UpdateGroupState();
            }

            // Draw non-serialized fields
            foreach (var field in this.nonSerializedFields)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)field.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                FieldDrawer drawer = FieldDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                drawer?.DrawField(this.target, field);
            }

            // Draw native properties
            foreach (var property in this.nativeProperties)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)property.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                NativePropertyDrawer drawer = NativePropertyDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                drawer?.DrawNativeProperty(this.target, property);
            }

            // Draw methods
            foreach (var method in this.methods)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)method.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                MethodDrawer methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                methodDrawer?.DrawMethod(this.target, method);
            }
        }

        protected void UpdateGroupState()
        {
            groupedState.Clear();

            foreach (var field in this.fields)
            {
                foreach (var group in field.GetCustomAttributes<DrawGroupConditionAttribute>(true))
                {
                    object value = field.GetValue(this.target);

                    bool canDraw = true;
                    if (group.compareObj == null && field.FieldType.IsAssignableFrom(typeof(IConvertible))) canDraw = Convert.ToBoolean(value);
                    else if (group.compareObj != null && group.compareObj is IComparable comp01) canDraw = comp01.CompareTo(value) == 0;
                    else continue;

                    foreach (var groupName in group.groups) groupedState[groupName] = canDraw && group.canDraw;
                }
            }
        }

        protected void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields) this.ValidateAndDrawField(field);
        }

        protected void ValidateAndDrawField(FieldInfo field)
        {
            if (!ShouldDrawField(field)) return;
            this.ValidateField(field);
            this.ApplyFieldMeta(field);
            this.DrawField(field);
        }

        protected void ValidateField(FieldInfo field)
        {
            ValidatorAttribute[] validatorAttributes = (ValidatorAttribute[])field.GetCustomAttributes(typeof(ValidatorAttribute), true);

            foreach (var attribute in validatorAttributes)
            {
                PropertyValidator validator = PropertyValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                if (validator != null) validator.ValidateProperty(this.serializedPropertiesByFieldName[field.Name]);
            }
        }

        protected bool ShouldDrawField(FieldInfo field)
        {
            // Check if the field has draw conditions
            PropertyDrawCondition drawCondition = this.GetPropertyDrawConditionForField(field);
            if (drawCondition != null)
            {
                bool canDrawProperty = drawCondition.CanDrawProperty(this.serializedPropertiesByFieldName[field.Name]);
                if (!canDrawProperty) return false;
            }

            // Check if the field has HideInInspectorAttribute
            HideInInspector[] hideInInspectorAttributes = (HideInInspector[])field.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0) return false;

            return true;
        }

        protected void DrawField(FieldInfo field)
        {
            EditorGUI.BeginChangeCheck();
            PropertyDrawer drawer = this.GetPropertyDrawerForField(field);
            if (drawer != null)
            {
                drawer.FieldInfo = field;
                drawer.DrawProperty(this.serializedPropertiesByFieldName[field.Name]);
                drawer.FieldInfo = null;
                drawer = null;
            }
            else EditorDrawUtility.DrawPropertyField(this.serializedPropertiesByFieldName[field.Name]);

            if (EditorGUI.EndChangeCheck())
            {
                OnValueChangedAttribute[] onValueChangedAttributes = (OnValueChangedAttribute[])field.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (var onValueChangedAttribute in onValueChangedAttributes)
                {
                    PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (meta != null) meta.ApplyPropertyMeta(this.serializedPropertiesByFieldName[field.Name], onValueChangedAttribute);
                }
            }
        }

        protected void ApplyFieldMeta(FieldInfo field)
        {
            // Apply custom meta attributes
            MetaAttribute[] metaAttributes = field
                .GetCustomAttributes(typeof(MetaAttribute), true)
                .Where(attr => attr.GetType() != typeof(OnValueChangedAttribute))
                .Select(obj => obj as MetaAttribute)
                .ToArray();

            Array.Sort(metaAttributes, (x, y) => x.Order - y.Order);

            foreach (var metaAttribute in metaAttributes)
            {
                PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(metaAttribute.GetType());
                if (meta != null) meta.ApplyPropertyMeta(this.serializedPropertiesByFieldName[field.Name], metaAttribute);
            }
        }

        protected PropertyDrawer GetPropertyDrawerForField(FieldInfo field)
        {
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                var attr = drawerAttributes[0].GetType();
                PropertyDrawer drawer = PropertyDrawerDatabase.GetDrawerForAttribute(attr);
                if (drawer == null)
                {
                    Debug.LogError($"DrawerAttribute: {attr.GetType().Name} is defined, but coresponding Drawer is missing. [Update Attribute Database] may fixed this.");
                    return null;
                }
                return drawer.Clone();
            }
            else return null;
        }

        protected PropertyGrouper GetPropertyGrouperForField(FieldInfo field)
        {
            GroupAttribute[] groupAttributes = (GroupAttribute[])field.GetCustomAttributes(typeof(GroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                PropertyGrouper grouper = PropertyGrouperDatabase.GetGrouperForAttribute(groupAttributes[0].GetType());
                return grouper;
            }
            else return null;
        }

        protected PropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo field)
        {
            DrawConditionAttribute[] drawConditionAttributes = (DrawConditionAttribute[])field.GetCustomAttributes(typeof(DrawConditionAttribute), true);
            if (drawConditionAttributes.Length > 0)
            {
                PropertyDrawCondition drawCondition = PropertyDrawConditionDatabase.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
                return drawCondition;
            }
            else return null;
        }
    }
}
