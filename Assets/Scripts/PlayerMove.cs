using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed, StunnedTime;
    private bool StunnedCheck = false;
    public KeyCode upKeyCode, downKeyCode, leftKeyCode, rightKeyCode;

    void Start() {
        
    }

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

    // stunned by sonar
    private void Awake() {
        DomainEvents.Register<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
    
    // ignore input and physics impacts
    private void OnPlayerTriggerEvent(OnPlayerTrigger param) {
        if (param.State == Sonar.SonarState.indirect) return;
        if (param.Player == transform.gameObject) return;
        transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        StunnedCheck = true;
        StartCoroutine(OnStunnedEnd());
    }
    private IEnumerator OnStunnedEnd() {
        yield return new WaitForSeconds(StunnedTime);
        transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        StunnedCheck = false;
    }
    private void OnDestroy() {
        DomainEvents.UnRegister<OnPlayerTrigger>(OnPlayerTriggerEvent);
    }
}
