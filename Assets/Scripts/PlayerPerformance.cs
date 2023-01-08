using System.Collections;
using System.Collections.Generic;
using BIGJ2023.Common;
using Scream.UniMO.Common;
using UnityEngine;
using Logger = Scream.UniMO.Common.Logger;

// generate TreasuePick event
public class OnTreasuePick : IDomainEvent
{
    public GameObject Player;
}

// generate TreasueDrop event
public class OnTreasueDrop : IDomainEvent
{
    public GameObject Player;
}

// generate PlayerAction event
public enum PlayerAction { detect, counter }

public class OnPlayerAction : IDomainEvent
{
    public GameObject player;
    public PlayerAction ActionState;
}

public class PlayerPerformance : MonoBehaviour
{
    private const string stunnedEndEffectName = "Healed";
    public KeyCode DetectKeyCode, CounterKeyCode;
    public GameObject[] players;
    public GameObject Treasure, GameManager;
    public int TreasureOwner = -1;
    private bool Active = true;
    private bool canUseSonar = true;

    private void Start()
    {
        TreasureOwner = -1;
    }
    private void Update()
    {
        // release sonar
        if(!canUseSonar) return;
        if (Active && Input.GetKeyDown(DetectKeyCode))
        {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = gameObject;
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }
        if (Active && Input.GetKeyDown(CounterKeyCode))
        {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = gameObject;
            eventParam.ActionState = PlayerAction.counter;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }
    }

    // pick treasure
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Treasure)
        {
            if (Treasure.transform.parent == GameManager.transform)
            {
                Treasure.transform.SetParent(transform);
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] == transform.gameObject)
                    {
                        TreasureOwner = i; break;
                    }
                }
                OnTreasuePick eventParam = new OnTreasuePick();
                eventParam.Player = transform.gameObject;
                DomainEvents.Raise<OnTreasuePick>(eventParam);
            }
        }
    }


    // drop treasure
    public void DropTreasure(GameObject Player)
    {
        Treasure.transform.SetParent(GameManager.transform);
        TreasureOwner = -1;
    }

    // hitten by sonar
    private void Awake()
    {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
        DomainEvents.Register<OnCDTrigger>(OnCDTriggerEvent);
    }

    private void OnCDTriggerEvent(OnCDTrigger param)
    {
        if(param.Player == gameObject)
        {
            canUseSonar = !param.IsCD;
            Debug.Log("can use sonar:" + param.IsCD);
        }
    }
    // to drop treasure
    private void OnPlayerTriggerEvent(OnPlayerTrigger param)
    {
        if (param.State != Sonar.SonarState.direct) return;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != param.Player && TreasureOwner == i)
            {
                DropTreasure(players[i]); break;
            }
        }
        if (IsSamePlayer(param))
        {
            return;
        }
        Active = false;
        StartCoroutine(OnStunnedEnd());
    }

    private bool IsSamePlayer(OnPlayerTrigger param)
    {
        return param.Player == gameObject;
    }

    private IEnumerator OnStunnedEnd()
    {
        float StunnedTime = transform.gameObject.GetComponent<PlayerMove>().StunnedTime;
        yield return new WaitForSeconds(StunnedTime);
        //TODO: Fix pos
        ParticleSystem fx = FxManager.Instance.GetEffect(stunnedEndEffectName);
        fx.gameObject.transform.position = transform.position;
        Active = true;
    }
    private void OnDestroy()
    {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
}
