using Units.StateManagement;
using UnityEditor;

namespace Assets.Editor
{
    [CustomEditor(typeof(AbstractStateManager), true)]
    public class AbstractStateManagerInspector : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (AbstractStateManager)target;

            const string fieldName = "State";

            if (script.State != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.EnumPopup(fieldName, script.State);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(fieldName, "null");
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}