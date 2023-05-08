using UnityEditor;
using UEditor = UnityEditor.Editor;

namespace TerrainHandles.Editor
{
    internal static class ProjectSettingsRegister
    {
        private static UEditor editor;
            
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/Terrain Handles", SettingsScope.Project)
            {
                guiHandler = ctx =>
                {
                    var settings = ProjectSettings.Instance();
                    UEditor.CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                }
            };
        }
    }
}
