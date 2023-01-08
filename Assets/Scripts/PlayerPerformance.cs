using System;
using System.Collections;
using System.Collections.Generic;
using BIGJ2023.Common;
using Scream.UniMO.Common;
using UnityEngine;
using Logger = Scream.UniMO.Common.Logger;
using Scream.UniMO.Utils;

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
    private const string GetTreasureAudioName = "GetTreasure";
    private const string DropTreasureAudioName = "DropTreasure";
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
        bool check = false;
        if (Active && Input.GetKeyDown(DetectKeyCode))
        {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = gameObject;
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
            check = true;
        }
        if (Active && Input.GetKeyDown(CounterKeyCode))
        {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = gameObject;
            eventParam.ActionState = PlayerAction.counter;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
            check = true;
        }
        if (check) {
            gameObject.GetComponent<Animator>().SetBool("Release", true);
            gameObject.GetComponent<PlayerMove>().StunnedCheck = true;
            DelayDo(1.5f, () => { 
                gameObject.GetComponent<Animator>().SetBool("Release", false);
                gameObject.GetComponent<PlayerMove>().StunnedCheck = false;
            });
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
                for (int i = 0; i < players.Length; i++) {
                    if (players[i] == transform.gameObject) {
                        TreasureOwner = i; break;
                    }
                }
                OnTreasuePick eventParam = new OnTreasuePick();
                eventParam.Player = transform.gameObject;
                DomainEvents.Raise<OnTreasuePick>(eventParam);
                FxManager.Instance.PlayAudio(GetTreasureAudioName);
                Debug.Log("Pick");
                gameObject.GetComponent<Animator>().SetBool("Yeahh", true);
                gameObject.GetComponent<PlayerMove>().StunnedCheck = true;
                DelayDo(1f, () => { 
                    gameObject.GetComponent<Animator>().SetBool("Yeahh", false);
                    gameObject.GetComponent<PlayerMove>().StunnedCheck = false;
                });
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
                DropTreasure(players[i]); 
                Debug.Log("Drop");
                FxManager.Instance.PlayAudio(DropTreasureAudioName);
                break;
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
        DomainEvents.UnRegister<OnCDTrigger>(OnCDTriggerEvent);
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
