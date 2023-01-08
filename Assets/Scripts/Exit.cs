using System;
using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using Scream.UniMO.Utils;
using UnityEngine;

public class OnWinnerDetermine : IDomainEvent
{
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Treasure)
        {
            return;
        }
        if (Treasure.transform.parent != other.transform)
        {
            Debug.Log(other.name + " need to find treasure!!");
            return;
        }
        other.gameObject.GetComponent<Animator>().SetBool("Yeahh", true);
        other.gameObject.GetComponent<PlayerMove>().StunnedCheck = true;
        Debug.Log(other.name + "wins!!");
        DelayDo(3.5f, () => { 
            other.gameObject.GetComponent<Animator>().SetBool("Yeahh", false);
            other.gameObject.GetComponent<PlayerMove>().StunnedCheck = false;
            OnWinnerDetermine eventParam = new OnWinnerDetermine();
            eventParam.Winner = other.gameObject;
            DomainEvents.Raise<OnWinnerDetermine>(eventParam);
        });
        

    }

    private void DelayDo(float delay, Action action)
    {
        MonoHelper.Instance.StartCoroutine(DelayDoImpl(delay, action));
    }

    private IEnumerator DelayDoImpl(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

}
