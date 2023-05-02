using UnityEngine;

namespace TerrainHandles
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T val) ? val : gameObject.AddComponent<T>();
        }

        public static void GetOrAddCachedComponent<T>(this GameObject gameObject, ref T field) where T : Component
        {
            if (field) return;
            field = gameObject.GetOrAddComponent<T>();
        }
    }
}