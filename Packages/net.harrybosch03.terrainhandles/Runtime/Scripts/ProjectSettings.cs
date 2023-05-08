using UnityEngine;

namespace TerrainHandles
{
    public partial class ProjectSettings : ScriptableObject
    {
        internal const string SaveLocation = "Packages/net.harrybosch03.terrainhandles/Runtime/Scriptable Objects/ProjectSettings.asset";

        public TerrainGenerationSettings fallbackSettings;
        public int updateFrequency = 10;
        public UpdateMethod updateMethod;

        public enum UpdateMethod
        {
            OnChangeEveryFrame,
            OnChangeFinalize,
            Never,
        }
        
        public TerrainGenerationSettings FallbackSettings => fallbackSettings;
    }
}