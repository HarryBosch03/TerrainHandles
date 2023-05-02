using UnityEditor;

namespace TerrainHandles
{
    public partial class Chunk
    {
#if UNITY_EDITOR
        static Chunk()
        {
            EditorApplication.hierarchyChanged += Dirtied;
        }

        [MenuItem("Actions/Terrain Handles/Regenerate All")]
        private static void RegenerateAllMenuItem()
        {
            RegenerateAll(true);
        }
        
        private static void Dirtied()
        {
            RegenerateAll();
        }
#endif
    }
}