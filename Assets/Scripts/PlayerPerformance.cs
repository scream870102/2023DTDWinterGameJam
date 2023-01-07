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
    public GameObject[] players;

    struct TreasureOwner {
        public GameObject Treasure;
        public GameObject Owner;
    };
    TreasureOwner TreasureOwnerCheck;
    
    private void Awake() {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }

    // drop treasure timing
    private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
        if(param.State != Sonar.SonarState.direct) {
            return;
        }
        if(TreasureOwnerCheck.Owner == param.Player) {
            DropTreasure(TreasureOwnerCheck.Treasure);
        }
    }

    private void OnDestroy() {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }

    public KeyCode[] DetectKeyCode, CounterKeyCode;
    private void Update() {
        // release sonar
        for(int i = 0; i < players.Length; i ++) {
            if(Input.GetKey(DetectKeyCode[i])) {
                OnPlayerAction eventParam = new OnPlayerAction();
                eventParam.player = players[i];
                eventParam.ActionState = PlayerAction.detect;
                DomainEvents.Raise<OnPlayerAction>(eventParam);
            }
            if(Input.GetKey(CounterKeyCode[i])) {
                OnPlayerAction eventParam = new OnPlayerAction();
                eventParam.player = players[i];
                eventParam.ActionState = PlayerAction.counter;
                DomainEvents.Raise<OnPlayerAction>(eventParam);
            }
        }
        
    }

    // pick treasure
    private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag == "Treasure") {
            int count = 0;
            Transform parent = other.transform.parent;
            while (parent != null) {
                count++;
                parent = parent.parent;
            }
            if (count <= 1) {
                other.transform.SetParent(transform);
                TreasureOwnerCheck.Owner = transform.gameObject;
                TreasureOwnerCheck.Treasure = other.gameObject;
                OnTreasuePick eventParam = new OnTreasuePick();
                eventParam.Player = transform.gameObject;
                DomainEvents.Raise<OnTreasuePick>(eventParam);
            }
        }
    }


    // drop treasure
    public void DropTreasure(GameObject Player) {
        Transform treasure = TreasureOwnerCheck.Treasure.transform;
        treasure.SetParent(null);
        OnTreasueDrop eventParam = new OnTreasueDrop();
        eventParam.Player = Player;
        DomainEvents.Raise<OnTreasueDrop>(eventParam);
    }
}
