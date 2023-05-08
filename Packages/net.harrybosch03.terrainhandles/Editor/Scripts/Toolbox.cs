using UnityEditor;
using UnityEngine;

namespace TerrainHandles.Editor
{
    public class Toolbox : EditorWindow
    {
        private UnityEditor.Editor projectSettingsEditor;

        private readonly EditorHelper helper = new();
        
        [MenuItem(EditorHelper.Window + EditorHelper.SubMenu + "Toolbox", priority = int.MaxValue)]
        public static void Open()
        {
            CreateWindow<Toolbox>("Toolbox");
        }

        private void OnGUI()
        {
            helper.Section("Project Settings", () =>
            {
                var projectSettings = ProjectSettings.Instance();
                UnityEditor.Editor.CreateCachedEditor(projectSettings, null, ref projectSettingsEditor);
                projectSettingsEditor.OnInspectorGUI();
            });
            
            helper.Section("Actions", () =>
            {
                if (GUILayout.Button("Rebuild All Dirty Chunks")) Chunk.RegenerateAll();
                if (GUILayout.Button("Force Rebuild All Chunks")) Chunk.RegenerateAll(true);
            });
        }
    }
}