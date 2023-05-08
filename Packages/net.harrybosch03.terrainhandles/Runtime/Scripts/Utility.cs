using System;
using UnityEngine;

namespace TerrainHandles
{
    public static partial class Utility
    {
        public static Vector3 ConstructVector(Func<Func<Vector3, float>, float> callback)
        {
            return new Vector3(callback(v => v.x), callback(v => v.y), callback(v => v.z));
        }
    }
}