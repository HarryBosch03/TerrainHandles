using UnityEditor;
using static UnityEditor.EditorGUI;
using static UnityEngine.GUILayout;
using UEditor = UnityEditor.Editor;

namespace TerrainHandles.Editor
{
    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsEditor : UEditor
    {
        private UEditor fallbackSettingsEditor;
        private bool foldout;

        private readonly EditorHelper helper = new();
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var projectSettings = target as ProjectSettings;
            if (!projectSettings) return;

            helper.Section("Fallback Settings", () =>
            {
                if (projectSettings.FallbackSettings)
                {
                    CreateCachedEditor(projectSettings.FallbackSettings, null, ref fallbackSettingsEditor);
                    indentLevel++;
                    fallbackSettingsEditor.OnInspectorGUI();
                    indentLevel--;
                }
                else if (Button("Use Default Fallback Settings"))
                {
                    projectSettings.GetOrCreateFallbackTerrainGenerationSettings();
                }
            
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            });
        }
    }
}