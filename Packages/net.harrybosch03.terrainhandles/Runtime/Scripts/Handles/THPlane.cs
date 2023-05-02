using UnityEngine;

namespace TerrainHandles.Handles
{
    public class THPlane : TerrainHandle
    {
        public override float Apply(float w, Vector3 point, TerrainData data)
        {
            return position.y - point.y;
        }
    }
}
