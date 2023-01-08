using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace BIGJ2023.Common
{
    public class FxRecycler : MonoBehaviour
    {
        void OnParticleSystemStopped()
        {
            LeanPool.Despawn(gameObject);
        }
    }

}
