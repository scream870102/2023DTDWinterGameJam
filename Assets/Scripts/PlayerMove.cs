using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private void Awake() {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }

    // detect state
    public float StunnedTime = 2f;
    private bool StunnedCheck = false;
    private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
        if(param.State == Sonar.SonarState.indirect || param.Player == transform.gameObject) {
            return;
        }
        float StartTime = Time.time, PassedTime = 0;
        while(PassedTime <= StunnedTime) {
            StunnedCheck = true;
            PassedTime = Time.time - StartTime;
        }
        StunnedCheck = false;
    }

    private void OnDestroy() {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
    
    public float MoveSpeed;
    void Start() {
        MoveSpeed = 5f;
    }

    public KeyCode upKeyCode, downKeyCode, leftKeyCode, rightKeyCode;
    void Update() {
        if(!StunnedCheck) {
            // detect direction
            float up, down, left, right;
            up    = Input.GetKey(upKeyCode)    ? +1f : 0f;
            down  = Input.GetKey(downKeyCode)  ? -1f : 0f;
            left  = Input.GetKey(leftKeyCode)  ? -1f : 0f;
            right = Input.GetKey(rightKeyCode) ? +1f : 0f;

            // movement
            Vector3 movement = new Vector3((left+right)*MoveSpeed*Time.deltaTime, (up+down)*MoveSpeed*Time.deltaTime, 0);
            transform.Translate(movement);
        }
    }
}
