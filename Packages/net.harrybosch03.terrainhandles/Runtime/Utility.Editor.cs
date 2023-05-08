using UnityEditor;
using UnityEngine;

namespace TerrainHandles
{
    public static partial class Utility
    {
        public static partial void GetPackageScriptableObject<T>(string filename, ref T field)
            where T : ScriptableObject;

#if UNITY_EDITOR
        public static partial void GetPackageScriptableObject<T>(string filename, ref T field)
            where T : ScriptableObject
        {
            if (field) return;

            field = AssetDatabase.LoadAssetAtPath<T>(filename);
            if (field) return;

            field = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(field, filename);
            AssetDatabase.SaveAssets();
        }
#else
        public static partial void GetPackageScriptableObject<T>(string filename, ref T field) where T : ScriptableObject { }
#endif
    }
}