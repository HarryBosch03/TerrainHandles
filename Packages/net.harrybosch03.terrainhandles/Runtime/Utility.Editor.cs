using UnityEditor;
using UnityEngine;

namespace TerrainHandles
{
    public static partial class Utility
    {
        public static partial void GetOrCreateScriptableObject<T>(string filename, ref T field)
            where T : ScriptableObject;

        public static partial T GetOrCreateScriptableObject<T>(string filename) where T : ScriptableObject;

#if UNITY_EDITOR
        public static partial void GetOrCreateScriptableObject<T>(string filename, ref T field)
            where T : ScriptableObject
        {
            if (field) return;

            field = AssetDatabase.LoadAssetAtPath<T>(filename);
            if (field) return;

            field = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(field, filename);
            AssetDatabase.SaveAssets();
        }

        public static partial T GetOrCreateScriptableObject<T>(string filename) where T : ScriptableObject
        {
            T val = null;
            GetOrCreateScriptableObject(filename, ref val);
            return val;
        }
#else
        public static partial void GetOrCreateScriptableObject<T>(string filename, ref T field) where T : ScriptableObject { }
        public static partial T GetOrCreateScriptableObject<T>(string filename) where T : ScriptableObject => null;
#endif
    }
}