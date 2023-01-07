using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scream.UniMO.Common;
using Lean.Pool;

namespace BIGJ2023.Common
{
    public class FxManager : MonoBehaviour
    {
        [SerializeField] private FxData database;

        #region Instance
        public static FxManager Instance { get; private set; } = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(this);
            }
        }
        #endregion Instance

        public ParticleSystem GetEffect(string fxName)
        {
            var collection = database.GetCollection();
            if (collection.ContainsKey(fxName))
            {
                return LeanPool.Spawn(collection[fxName]).GetComponent<ParticleSystem>();
            }
            return null;
        }

        public void RecycleEffect(GameObject toRecycle)
        {
            LeanPool.Despawn(toRecycle);
        }
    }

}
