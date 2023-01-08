using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scream.UniMO.Common;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [SerializeField] private GameObject[] players;
    public GameObject[] Players => players;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(players[0].transform.position, players[1].transform.position);
    }

    public int GetPlayerIndex(GameObject playerObj)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == playerObj)
            {
                return i;
            }
        }
        return -1;
    }
}
