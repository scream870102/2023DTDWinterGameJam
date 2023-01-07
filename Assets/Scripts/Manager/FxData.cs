using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scream.UniMO.Collections;
using System;
using Lean.Pool;

namespace BIGJ2023.Common
{
    [System.Serializable]
    public class FxContainer : Container<string, GameObject> { }

    [CreateAssetMenu(fileName = "FxData", menuName = "Misc/FxData")]
    public class FxData : ScriptableObject
    {
        private Dictionary<string, GameObject> data;
        [SerializeField] private FxContainer[] rawData;



        public Dictionary<string, GameObject> GetCollection()
        {
            if (data == default)
            {
                data = rawData.ToDictionary();
            }
            return data;
        }
    }

}
