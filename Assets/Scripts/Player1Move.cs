using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class Player1Move : MonoBehaviour
{
    private bool StunnedCheck = false;
    public float MoveSpeed;

    // private void Awake() {
    //     DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    // }

    // detect state
    // public float StunnedTime = 2f;
    // private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
    //     if(param.SonarState == 1 || param.player.gameObject.tag != "player1") {
    //         return;
    //     }
    //     float StartTime = Time.time, PassedTime = 0;
    //     while(PassedTime <= StunnedTime) {
    //         StunnedCheck = true;
    //         PassedTime = Time.time - StartTime;
    //     }
    //     StunnedCheck = false;
    // }

    // private void OnDestroy() {
    //     DomainEvents.UnRegiste<OnPlayerTrigger>(OnPlayerTriggerEvent);
    // }

    void Start() {
        MoveSpeed = 5f;
    }

    void Update() {
        if(!StunnedCheck) {
            // detect direction
            float up, down, left, right;
            up    = Input.GetKey("w") ? +1f : 0f;
            down  = Input.GetKey("s") ? -1f : 0f;
            left  = Input.GetKey("a") ? -1f : 0f;
            right = Input.GetKey("d") ? +1f : 0f;

            // movement
            Vector3 movement = new Vector3((left+right)*MoveSpeed*Time.deltaTime, (up+down)*MoveSpeed*Time.deltaTime, 0);
            transform.Translate(movement);
        }
    }

}
