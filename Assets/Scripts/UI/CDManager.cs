using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scream.UniMO.Common;
using Scream.UniMO.Utils;

public class OnCDTrigger : IDomainEvent
{
    public GameObject Player { get; private set; }
    public bool IsCD { get; private set;}

    public OnCDTrigger(GameObject player, bool isCD)
    {
        Player = player;
        IsCD = isCD;
    }
}

public class CDManager : MonoBehaviour
{
    [SerializeField]private float detectCDDuration;
    [SerializeField]private float counterCDDuration;
    [SerializeField]private Image p1DetectCD;
    [SerializeField]private Image p2DetectCD;
    [SerializeField]private Image p1CounterCD;
    [SerializeField]private Image p2CounterCD;
    private ScaledTimer p1CDTimer= new ScaledTimer();
    private ScaledTimer p2CDTimer= new ScaledTimer();
    private bool p1InCD = false;
    private bool p2InCD = false;

    private void Awake()
    {
        DomainEvents.Register<OnPlayerAction>(OnPlayerActionEvent);
    }


    private void Update() {
        if(p1InCD)
        {
            if(!p1CDTimer.IsFinished)
            {
                p1DetectCD.fillAmount = p1CDTimer.Remain01;
                p1CounterCD.fillAmount = p1CDTimer.Remain01;
            }
            else
            {
                p1InCD = false;
                DomainEvents.Raise(new OnCDTrigger(PlayerManager.Instance.Players[0],false));
                //player can do action
            }
        }


        if(p2InCD)
        {
            if(!p2CDTimer.IsFinished)
            {
                p2DetectCD.fillAmount = p2CDTimer.Remain01;
                p2CounterCD.fillAmount = p2CDTimer.Remain01;
            }
            else
            {
                p2InCD = false;
                DomainEvents.Raise(new OnCDTrigger(PlayerManager.Instance.Players[1],false));
                //player can do action
            }

        }
    }

    public void OnPlayerActionEvent(OnPlayerAction param)
    {
        
        if(param.ActionState == PlayerAction.detect)
        {
            UseSonar(param.player, detectCDDuration);
        }
        else if(param.ActionState == PlayerAction.counter)
        {
            UseSonar(param.player, counterCDDuration);
        }
    }

    public void UseSonar(GameObject player, float CDtime)
    {
        if(player == PlayerManager.Instance.Players[0])
        {
            p1CDTimer.Reset(CDtime);
            DomainEvents.Raise(new OnCDTrigger(player,true));
            //player cannot do action
            p1InCD = true;
        }
        else if(player == PlayerManager.Instance.Players[1])
        {
            p2CDTimer.Reset(CDtime);
            DomainEvents.Raise(new OnCDTrigger(player,true));
            p2InCD = true;
        }
    }

}
