using Revy.Framework;
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {

            UnityEngine.GUI.enabled = false;
            EditorGUI.PropertyField(position, property, true);
            UnityEngine.GUI.enabled = true;
        }
    }
}