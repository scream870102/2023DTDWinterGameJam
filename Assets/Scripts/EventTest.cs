using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;


public class EventTest : MonoBehaviour {
    private void Awake() {
        DomainEvents.Register<OnTreasueDrop>(OnTreasueDropEvent);
        DomainEvents.Register<OnTreasuePick>(OnTreasuePickEvent);
        DomainEvents.Register<OnPlayerAction>(OnPlayerActionEvent);
    }

    private void OnTreasueDropEvent(OnTreasueDrop param) {
        Debug.Log($"{param.Player.name} Drop");
    }
    
    private void OnTreasuePickEvent(OnTreasuePick param) {
        Debug.Log($"{param.Player.name} Pick");
    }

    private void OnPlayerActionEvent(OnPlayerAction param) {
        Debug.Log($"{param.Player.name} Pick");
    }

    private void OnDestroy() {
        DomainEvents.UnRegister<OnTreasueDrop>(OnTreasueDropEvent);
        DomainEvents.UnRegister<OnTreasuePick>(OnTreasuePickEvent);
        DomainEvents.UnRegister<OnPlayerAction>(OnPlayerActionEvent);
    }
}
