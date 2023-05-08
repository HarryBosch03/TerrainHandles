using UnityEngine;

namespace TerrainHandles.Handles
{
    [SelectionBase]
    public abstract class TerrainHandle : MonoBehaviour
    {
        [SerializeField] private int order;
        [SerializeField] private float bleed;

        protected Matrix4x4 localToWorldMatrix;
        protected Matrix4x4 worldToLocalMatrix;
        
        public int Order => order;
        public abstract float AffectedSize { get; }

        protected Vector3 Position => localToWorldMatrix.GetColumn(3);
        protected Vector3 Up => localToWorldMatrix.GetColumn(1);
        
        private Vector3 MatrixByPoint(Matrix4x4 mat, Vector3 p, float w) => mat * new Vector4(p.x, p.y, p.z, w);
        protected Vector3 LocalToWorld(Vector3 p, float w = 1.0f) => MatrixByPoint(localToWorldMatrix, p, w);
        protected Vector3 WorldToLocal(Vector3 p, float w = 1.0f) => MatrixByPoint(worldToLocalMatrix, p, w);
        
        public virtual void Prepare()
        {
            localToWorldMatrix = transform.localToWorldMatrix;
            worldToLocalMatrix = transform.worldToLocalMatrix;
        }
        
        public abstract float Apply(float w, Vector3 point, TerrainData data);

        protected float Blend(float a, float b)
        {
            if (bleed == 0.0f) return Mathf.Min(a, b);
            var h = Mathf.Max(bleed - Mathf.Abs(a - b), 0.0f) / bleed;
            return Mathf.Min(a, b) - h * h * h * bleed / 6.0f;
        }
    }
}