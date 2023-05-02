using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace TerrainHandles
{
    public partial class Chunk : MonoBehaviour
    {
        [SerializeField] private Vector3 genSize;
        [SerializeField] private float voxelSize;
        [SerializeField] private TerrainGenerationSettings settings;

        private MeshFilter filter;

        public bool Dirty { get; set; }

        [ContextMenu("Generate")]
        public void Generate() => Generate(new TerrainData());

        public void Generate(TerrainData data)
        {
            StartCoroutine(GenerateRoutine(data, (gen, c1, c2, size) =>
            {
                gen.Generate(c1, c2, size);
                return null;
            }));
        }

        public void GenerateAsync(TerrainData data)
        {
            StartCoroutine(GenerateRoutine(data, (gen, c1, c2, size) =>
            {
                var thread = gen.GenerateAsync(c1, c2, size);
                return new WaitUntil(() => !thread.IsAlive);
            }));
        }

        private IEnumerator GenerateRoutine(TerrainData data,
            System.Func<MarchingCubes, Vector3, Vector3, float, IEnumerator> generateCallback)
        {
            if (voxelSize < 0.0001f) yield break;

            gameObject.GetOrAddCachedComponent(ref filter);
            if (!settings) settings = TerrainGenerationSettings.Fallback();

            var generator = new MarchingCubes(data.AtPoint, settings.threshold, transform.position);
            var halt = generateCallback(generator, -genSize * 0.5f, genSize * 0.5f, voxelSize);
            if (halt != null) yield return halt;

            var mesh = generator.BuildMesh();

            filter.sharedMesh = mesh;
        }

        public static void RegenerateAll(bool force = false)
        {
            var data = new TerrainData();
            foreach (var chunk in data.Chunks)
            {
                if (!chunk.Dirty && !force) continue;
                chunk.GenerateAsync(data);
                chunk.Dirty = false;
            }
        }
    }
}