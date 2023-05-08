using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TerrainHandles
{
    public class MarchingCubes
    {
        public readonly List<Vector3> vertices = new();
        public readonly List<Vector3> normals = new();
        public readonly List<int> indices = new();
        public readonly List<Vector2> uvs = new();
        public readonly List<Color> colors = new();
        public readonly Vector3 offset;
        public WeightCallback weight;
        public float threshold;
        public bool invert;

        private Vector3 start, end;
        private Vector3Int index;
        private float voxelSize;

        public MarchingCubes(WeightCallback weight, float threshold, Vector3 offset, bool invert = false)
        {
            this.weight = weight;
            this.threshold = threshold;
            this.offset = offset;
            this.invert = invert;
        }

        private readonly Dictionary<Vector3Int, int> sharedVertexMap = new();

        public float Weight(Vector3 p) => weight(p + offset);

        private bool State(float w) => w < threshold == !invert;

        private const int Corners = 8;
        private Vector3 Corner(int i)
        {
            var ci = CornerI(i);
            return (Vector3)ci * voxelSize + start;
        }
        
        private Vector3Int CornerI(int i)
        {
            return i switch
            {
                0 => index,
                1 => index + Vector3Int.right,
                2 => index + Vector3Int.right + Vector3Int.forward,
                3 => index + Vector3Int.forward,
                
                4 => index + Vector3Int.up,
                5 => index + Vector3Int.up + Vector3Int.right,
                6 => index + Vector3Int.up + Vector3Int.right + Vector3Int.forward,
                7 => index + Vector3Int.up + Vector3Int.forward,
                
                _ => throw new IndexOutOfRangeException()
            };
        }

        public void AddVertex(Vector3 vert, Vector3 reference)
        {
            var i = Vector3Int.RoundToInt(reference * 2.0f / voxelSize);
            if (sharedVertexMap.ContainsKey(i))
            {
                indices.Add(sharedVertexMap[i]);
                return;
            }
            
            vertices.Add(vert);
            sharedVertexMap.Add(i, vertices.Count - 1);
            indices.Add(vertices.Count - 1);

            CalculateSDFNormal(vert);
        }

        private void CalculateSDFNormal(Vector3 p)
        {
            var corners = new[]
            {
                new Vector3( 1.0f, -1.0f, -1.0f).normalized,
                new Vector3(-1.0f, -1.0f, -1.0f).normalized,
                new Vector3( 1.0f,  1.0f, -1.0f).normalized,
                new Vector3(-1.0f,  1.0f, -1.0f).normalized,
                new Vector3(-1.0f, -1.0f,  1.0f).normalized,
                new Vector3( 1.0f, -1.0f,  1.0f).normalized,
                new Vector3( 1.0f,  1.0f,  1.0f).normalized,
                new Vector3(-1.0f,  1.0f,  1.0f).normalized,
            };

            var normal = Vector3.zero;
            for (var i = 0; i < 8; i++)
            {
                normal += corners[i] * Weight(p + corners[i] * voxelSize * 0.25f);
            }
            normals.Add(normal.normalized);
        }

        private void Point()
        {
            var weights = new float[Corners];
            var config = 0b0u;
            
            for (var i = 0; i < weights.Length; i++)
            {
                var w = Weight(Corner(i));
                weights[i] = w;
                if (State(w)) config |= 1u << i;
            }

            if (config == 0b0) return;

            for (var i = 0; i < MarchingCubesIndices.Triangulation.GetLength(1); i++)
            {
                var edge = MarchingCubesIndices.Triangulation[config, i];
                if (edge == -1) break;

                var a = MarchingCubesIndices.CornerIndexFromEdge[edge, 0];
                var b = MarchingCubesIndices.CornerIndexFromEdge[edge, 1];

                var wa = weights[a];
                var wb = weights[b];

                var t = Mathf.InverseLerp(wa, wb, threshold);
                var c = Vector3.Lerp(Corner(a), Corner(b), t);
                var r = Vector3.Lerp(Corner(a), Corner(b), 0.5f);
                AddVertex(c, r);
            }
        }

        public Thread GenerateAsync(Vector3 start, Vector3 end, float voxelSize)
        {
            var thread = new Thread(() => Generate(start, end, voxelSize));
            thread.Start();
            return thread;
        }
        
        public void Generate(Vector3 start, Vector3 end, float voxelSize)
        {
            this.start = start;
            this.end = end;
            this.voxelSize = voxelSize;

            var xl = (int)((end.x - start.x) / voxelSize);
            var yl = (int)((end.y - start.y) / voxelSize);
            var zl = (int)((end.z - start.z) / voxelSize);

            for (var x = 0; x < xl; x++)
            {
                for (var y = 0; y < yl; y++)
                {
                    for (var z = 0; z < zl; z++)
                    {
                        index = new Vector3Int(x, y, z);
                        Point();
                    }
                }
            }
        }
        
        public Mesh BuildMesh()
        {
            var mesh = new Mesh();
            
            mesh.name = UnityEngine.Random.Range(0, 500).ToString();
            
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.SetUVs(0, uvs);
            mesh.SetColors(colors);

            return mesh;
        }

        public delegate float WeightCallback(Vector3 point);
    }
}