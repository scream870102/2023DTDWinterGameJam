using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class OnWinnerDetermine :IDomainEvent {
    public GameObject Winner;
}

public class Exit : MonoBehaviour
{
    public GameObject Treasure;
    public GameObject[] Players;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == Treasure) {
            return;
        }
        if (Treasure.transform.parent != other.transform) {
            Debug.Log(other.name + " need to find treasure!!");
            return;
        }
        Debug.Log(other.name + "wins!!");
        Time.timeScale = 0f;
        OnWinnerDetermine eventParam = new OnWinnerDetermine();
        eventParam.Winner = other.gameObject;
        DomainEvents.Raise<OnWinnerDetermine>(eventParam);
    }

}
