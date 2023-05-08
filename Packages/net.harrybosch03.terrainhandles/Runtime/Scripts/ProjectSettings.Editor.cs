#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TerrainHandles
{
    public partial class ProjectSettings
    {
        private const string FallbackTerrainGenerationSettingsFilename = "Packages/net.harrybosch03.terrainhandles/Runtime/Scriptable Objects/FallbackTerrainGenerationSettings.asset";

        public static partial ProjectSettings Instance();
        public partial void GetOrCreateFallbackTerrainGenerationSettings();

        private static ProjectSettings instance;

        private void Awake()
        {
            instance = this;
        }

#if UNITY_EDITOR
        public static partial ProjectSettings Instance()
        {
            Utility.GetPackageScriptableObject(SaveLocation, ref instance);
            return instance;
        }

        public partial void GetOrCreateFallbackTerrainGenerationSettings()
        {
            if (fallbackSettings) return;

            fallbackSettings = AssetDatabase.LoadAssetAtPath<TerrainGenerationSettings>(FallbackTerrainGenerationSettingsFilename);
            if (fallbackSettings) return;

            fallbackSettings = CreateInstance<TerrainGenerationSettings>();
            AssetDatabase.CreateAsset(fallbackSettings, FallbackTerrainGenerationSettingsFilename);
            AssetDatabase.SaveAssets();
        }
#else
        public partial void GetOrCreateFallbackTerrainGenerationSettings() { }
        public static partial ProjectSettings Instance() => instance;
#endif
    }
}