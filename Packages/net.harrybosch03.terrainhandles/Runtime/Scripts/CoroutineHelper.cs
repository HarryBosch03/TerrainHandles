using System;
using System.Collections;
using UnityEngine;

namespace TerrainHandles
{
    public class CoroutineHelper : IDisposable
    {
        private readonly CoroutineHelperBehaviour ctx;

        public CoroutineHelper()
        {
            ctx = new GameObject("Coroutine Helper").AddComponent<CoroutineHelperBehaviour>();
            ctx.hideFlags = HideFlags.HideAndDontSave;
        }
        
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return ctx.StartCoroutine(coroutine);
        }

        public void Dispose()
        {
            UnityEngine.Object.DestroyImmediate(ctx.gameObject);
        }
    }
}