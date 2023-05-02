using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
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
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var projectSettings = target as ProjectSettings;
            if (!projectSettings) return;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            using (new IndentLevelScope())
            {
                foldout = Foldout(foldout, "Fallback Settings");
                if (!foldout) return;
                
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
            }
        }
    }
}