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
        [SerializeField] private GameObject audioPlayerPrefab;
        private AudioSource audioPlayer;

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
        private void Start()
        {
            var audiogo = Instantiate(audioPlayerPrefab);
            audiogo.transform.SetParent(transform);
            audioPlayer = audiogo.GetComponent<AudioSource>();
        }

        public ParticleSystem GetEffect(string fxName)
        {
            var collection = database.GetCollection();
            if (collection.ContainsKey(fxName))
            {
                return LeanPool.Spawn(collection[fxName]).GetComponent<ParticleSystem>();
            }
            Debug.Log("missing effect " + fxName);
            return null;
        }

        public void RecycleEffect(GameObject toRecycle)
        {
            LeanPool.Despawn(toRecycle);
        }

        public void PlayAudio(string fxName)
        {
            var collection = database.GetAudioCollection();
            if (collection.ContainsKey(fxName))
            {
                //var player = LeanPool.Spawn(database.GetAudioPlayerPrefab()).GetComponent<AudioSource>();
                //udioPlayer.clip = collection[fxName]
                audioPlayer.PlayOneShot(collection[fxName]);
                return;
            }
            Debug.Log("missing audio " + fxName);
        }
    }

}
