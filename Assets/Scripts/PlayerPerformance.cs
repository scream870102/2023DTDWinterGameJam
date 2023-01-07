using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

// generate TreasuePick event
public class OnTreasuePick : IDomainEvent {
    public GameObject Player;
}

// generate TreasueDrop event
public class OnTreasueDrop : IDomainEvent {
    public GameObject Player;
}

// generate PlayerAction event
public enum PlayerAction {detect,counter}

public class OnPlayerAction : IDomainEvent {
    public GameObject player;
    public PlayerAction ActionState;
}

public class PlayerPerformance : MonoBehaviour {

    private void Awake() {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }

    // drop treasure timing
    private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
        if(param.State == Sonar.SonarState.direct) {
            DropTreasure(param.Player.gameObject);
        }
    }

    private void OnDestroy() {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }

    private void Start() {
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public GameObject[] players;
    public KeyCode[] DetectKeyCode, CounterKeyCode;
    private void Update() {

        // release sonar
        if(Input.GetKey(DetectKeyCode[0])) {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = players[0];
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey(CounterKeyCode[0])) {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = players[0];
            eventParam.ActionState = PlayerAction.counter;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey(DetectKeyCode[1])) {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = players[1];
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey(CounterKeyCode[1])) {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = players[1];
            eventParam.ActionState = PlayerAction.counter;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }
    }


    // pick treasure
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Treasure") {
            int count = 0;
            Transform parent = other.transform.parent;
            while (parent != null) {
                count++;
                parent = parent.parent;
            }
            if (count <= 1) {
                other.transform.SetParent(transform);
                OnTreasuePick eventParam = new OnTreasuePick();
                eventParam.Player = transform.gameObject;
                DomainEvents.Raise<OnTreasuePick>(eventParam);
            }
        }
    }


    // drop treasure
    public void DropTreasure(GameObject Player) {
        if (Player.transform.childCount == 0) return;
        Transform treasure = Player.transform.GetChild(0);
        treasure.SetParent(null);
        OnTreasueDrop eventParam = new OnTreasueDrop();
        eventParam.Player = Player;
        DomainEvents.Raise<OnTreasueDrop>(eventParam);
    }
}
