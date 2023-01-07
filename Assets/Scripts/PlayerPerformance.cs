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

    // private void Awake() {
    //     DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    // }

    // drop treasure timing
    // private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
    //     if(param.SonarState == 0) {
    //         DropTreasure(param.player.gameObject);
    //     }
    // }

    // private void OnDestroy() {
    //     DomainEvents.UnRegiste<OnPlayerTrigger>(OnPlayerTriggerEvent);
    // }

    private void Start() {
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void Update() {

        // release sonar
        if(Input.GetKey("e") && transform.gameObject.tag == "Player1") {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = transform.gameObject;
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey("r") && transform.gameObject.tag == "Player1") {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = transform.gameObject;
            eventParam.ActionState = PlayerAction.counter;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey("o") && transform.gameObject.tag == "Player2") {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = transform.gameObject;
            eventParam.ActionState = PlayerAction.detect;
            DomainEvents.Raise<OnPlayerAction>(eventParam);
        }

        if(Input.GetKey("p") && transform.gameObject.tag == "Player2") {
            OnPlayerAction eventParam = new OnPlayerAction();
            eventParam.player = transform.gameObject;
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
