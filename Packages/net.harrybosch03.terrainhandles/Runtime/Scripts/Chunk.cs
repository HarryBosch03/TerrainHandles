using System.Collections;
using System.Threading;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TerrainHandles
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private Vector3 genSize;
        [SerializeField] private float voxelSize;
        [SerializeField] private TerrainGenerationSettings settings;

        private MeshFilter filter;
        private Thread generationThread;
        private bool pendingGeneration;

        public Vector3 GenSize => genSize;

        public bool Dirty { get; set; }

        [ContextMenu("Generate")]
        public void Generate() => Generate(new TerrainData());

        public bool FinishedGeneration => generationThread != null && !generationThread.IsAlive && pendingGeneration;

        public Bounds Bounds => new(transform.position, genSize);

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
            if (generationThread != null && generationThread.IsAlive) return;
            
            StartCoroutine(GenerateRoutine(data, (gen, c1, c2, size) =>
            {
                generationThread = gen.GenerateAsync(c1, c2, size);
                pendingGeneration = true;
                return new WaitUntil(() => !generationThread.IsAlive);
            }));
        }

        public void Pregenerate(out MarchingCubes generator, out Vector3 min, out Vector3 max)
        {
            
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
            EditorUtility.SetDirty(gameObject);
            print("Done");

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