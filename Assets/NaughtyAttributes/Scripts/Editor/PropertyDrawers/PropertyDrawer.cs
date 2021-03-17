using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyDrawer
    {
        public abstract void DrawProperty(SerializedProperty property);
        public FieldInfo FieldInfo { get; internal set; }

        internal void ClearCache()
        {
            FieldInfo = null;
            OnClearCache();
        }

        public virtual void OnClearCache() { }
        public PropertyDrawer Clone() => (PropertyDrawer)this.MemberwiseClone();
        public PropertyDrawer Clone<T>() where T : PropertyDrawer
        => (T)this.MemberwiseClone();
    }
}
