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
    [System.Serializable]
    public class AudioContainer : Container<string, AudioClip> { }

    [CreateAssetMenu(fileName = "FxData", menuName = "Misc/FxData")]
    public class FxData : ScriptableObject
    {
        private Dictionary<string, GameObject> data;
        [SerializeField] private FxContainer[] rawData;

        private Dictionary<string, AudioClip> audioData;
        [SerializeField] private AudioContainer[] rawAudioData;

        [SerializeField] private GameObject audioPlayerPrefab;



        public Dictionary<string, GameObject> GetCollection()
        {
            if (data == default)
            {
                data = rawData.ToDictionary();
            }
            return data;
        }

        public GameObject GetAudioPlayerPrefab()
        {
            if(audioPlayerPrefab != null)
            {
                return audioPlayerPrefab;
            }
            return null;
        }
        public Dictionary<string, AudioClip> GetAudioCollection()
        {
            if (audioData == default)
            {
                audioData = rawAudioData.ToDictionary();
            }
            return audioData;
        }
    }

}
