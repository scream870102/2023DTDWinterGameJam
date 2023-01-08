using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    struct Points {
        public Transform[] Pos;
    }
    [SerializeField] private Points[] SpawnPoints;
    
    public Transform treasure;
    void Start()
    {
        // set the position of players in random
        int PointId = Random.Range(0, SpawnPoints.Length);
        int size = SpawnPoints[PointId].Pos.Length;
        for(int i = 0; i < size; i ++) {
            int id = Random.Range(0, SpawnPoints[PointId].Pos.Length);
            Transform tmp = SpawnPoints[PointId].Pos[id];
            SpawnPoints[PointId].Pos[id] = SpawnPoints[PointId].Pos[i];
            SpawnPoints[PointId].Pos[i] = tmp;
        }

        for(int i = 0; i < PlayerManager.Instance.Players.Length; i ++) {
            PlayerManager.Instance.Players[i].transform.position = SpawnPoints[PointId].Pos[i].position;
        }
        // set the position of treasure in random
        treasure.position = SpawnPoints[PointId].Pos[size - 1].position;
    }


    void Update()
    {

    }

}
