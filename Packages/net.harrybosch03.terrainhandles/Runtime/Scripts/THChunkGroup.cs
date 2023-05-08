using System;
using UnityEditor;
using UnityEngine;

namespace TerrainHandles
{
    public class THChunkGroup : MonoBehaviour
    {
        [SerializeField] private Vector3Int chunkCount;
        [SerializeField] private Chunk prefab;

        [ContextMenu("Populate")]
        public void Populate()
        {
            Undo.RecordObject(transform, "Terrain Chunks Populate");
            
            while(transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            var chunkSize = prefab.GenSize;

            var p = Vector3Int.zero;
            
            float getPosComponent(Func<Vector3, float> axis)
            {
                return (axis(p) - (axis(chunkCount) - 1) / 2.0f) * axis(chunkSize);
            }

            while (p.z < chunkCount.z)
            {
                var chunk = (Chunk)PrefabUtility.InstantiatePrefab(prefab);
                var pos = Utility.ConstructVector(getPosComponent);

                chunk.transform.SetParent(transform);
                chunk.transform.localPosition = pos;
                chunk.transform.localRotation = Quaternion.identity;
                chunk.transform.localScale = Vector3.one;
                chunk.Dirty = true;
                
                p.x++;
                if (p.x >= chunkCount.x)
                {
                    p.x = 0;
                    p.y++;
                }
                if (p.y >= chunkCount.y)
                {
                    p.y = 0;
                    p.z++;
                }
            }

            Chunk.RegenerateAll();
        }
    }
}
