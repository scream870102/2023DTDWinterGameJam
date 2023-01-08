using System;
using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using Scream.UniMO.Utils;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed, StunnedTime;
    public bool StunnedCheck = false;
    public KeyCode upKeyCode, downKeyCode, leftKeyCode, rightKeyCode;

    void Start() {
        StunnedTime = 4f;
    }

    void Update() {
        transform.gameObject.GetComponent<Animator>().SetBool("Shift", false);
        if(!StunnedCheck) {
            // detect direction
            float up, down, left, right;
            up    = Input.GetKey(upKeyCode)    ? +1f : 0f;
            down  = Input.GetKey(downKeyCode)  ? -1f : 0f;
            left  = Input.GetKey(leftKeyCode)  ? -1f : 0f;
            right = Input.GetKey(rightKeyCode) ? +1f : 0f;
            
            if((up == 1f) || (down == -1f) || (left == -1f) || (right == 1f)) {
                transform.gameObject.GetComponent<Animator>().SetBool("Shift", true);
            }
            // movement
            Vector3 movement = new Vector3((left+right)*MoveSpeed*Time.deltaTime, (up+down)*MoveSpeed*Time.deltaTime, 0);
            transform.Translate(movement);
        }
    }

    // stunned by sonar
    private void Awake() {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
    
    // ignore input and physics impacts
    private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
        if (param.State == Sonar.SonarState.indirect) return;
        if (param.Player == transform.gameObject) return;
        transform.gameObject.GetComponent<Animator>().SetBool("Stunned", true);
        transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        StunnedCheck = true;
        StartCoroutine(OnStunnedEnd());
    }
    private IEnumerator OnStunnedEnd() {
        yield return new WaitForSeconds(StunnedTime);
        transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        StunnedCheck = false;
        transform.gameObject.GetComponent<Animator>().SetBool("Stunned", false);
    }
    private void OnDestroy() {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
}
