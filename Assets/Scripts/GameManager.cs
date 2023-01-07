using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    struct PointPair {
        public Transform PointA;
        public Transform PointB;
        public Transform TreasurePoint;
    }
    [SerializeField] private List<PointPair> _spawnPoints = new List<PointPair>();
    
    void Start()
    {
        // set the position of players and treasure
        for (int i = 0; i < transform.childCount; i++) {
            string tag = transform.GetChild(i).gameObject.tag;
            int pos = Random.Range(0, _spawnPoints.Count);
              if (tag == "Player1") {
                transform.GetChild(i).gameObject.transform.position = _spawnPoints[pos].PointA.position;
            } if (tag == "Player2") {
                transform.GetChild(i).gameObject.transform.position = _spawnPoints[pos].PointB.position;
            } if (tag == "Treasure") {
                transform.GetChild(i).gameObject.transform.position = _spawnPoints[pos].TreasurePoint.position;
            }

        }
    }


    void Update()
    {

    }


}