using UnityEngine;

namespace TerrainHandles.Handles
{
    [SelectionBase]
    public abstract class TerrainHandle : MonoBehaviour
    {
        [SerializeField] private int order;

        protected Vector3 position;
        
        public int Order => order;

        public virtual void Prepare()
        {
            position = transform.position;
        }
        
        public abstract float Apply(float w, Vector3 point, TerrainData data);
    }
}