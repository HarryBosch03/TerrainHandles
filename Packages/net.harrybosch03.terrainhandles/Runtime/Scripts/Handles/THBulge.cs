using UnityEngine;

namespace TerrainHandles.Handles
{
    public class THBulge : TerrainHandle
    {
        [SerializeField] private float radius;
        
        public override float Apply(float w, Vector3 point, TerrainData data)
        {
            var x = (point - position).magnitude;
            return w + Mathf.Max(radius - x, 0.0f);
        }
    }
}
