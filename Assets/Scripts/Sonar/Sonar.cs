using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scream.UniMO;
using Scream.UniMO.Common;
using System;
using BIGJ2023.GameSystem;
using Scream.UniMO.Utils;

public class OnPlayerTrigger : IDomainEvent
{
    public Sonar.SonarState State { get; private set; }
    public GameObject Player { get; private set; }

    public OnPlayerTrigger(Sonar.SonarState state, GameObject player)
    {
        State = state;
        Player = player;
    }
}

public class Sonar : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Player
    {
        player1,
        player2
    }

    public enum SonarState
    {
        direct,
        indirect
    }
    [SerializeField] private float sonarMinRadius = 0.5f;
    [SerializeField] private float sonarMaxRadius = 3;
    [SerializeField] private float sonarStayDuration = 0.5f;
    [SerializeField] private float sonarMaskDetectSpeed = 0.5f;
    [SerializeField] private LayerMask detectLayer;

    private GameObject sonar;
    private CircleCollider2D sonarCollider;
    public bool IsSonarOpen { get; private set; }

    private Coroutine playingSonar;
    private GameObject parent;

    private void Awake()
    {
        sonarCollider = GetComponent<CircleCollider2D>();
        DomainEvents.Register<OnPlayerAction>(OnPlayerActionEvent);
    }

    private void OnDestroy()
    {
        DomainEvents.UnRegister<OnPlayerAction>(OnPlayerActionEvent);
    }

    private void OnPlayerActionEvent(OnPlayerAction param)
    {
        if (param.ActionState == PlayerAction.detect && param.player == parent)
        {
            Detect();
        }
        else if (param.ActionState == PlayerAction.counter && param.player == parent)
        {
            Counter();
        }
    }

    private void OnPlayerTriggerEvent(OnPlayerTrigger param) { }

    public void Init(GameObject parent)
    {
        this.parent = parent;
        sonar = gameObject;
        SetSonarOpen(false);
    }

    private void Detect()
    {
        if (sonarCollider == null) return;
        if (IsSonarOpen) return;
        //TODO: -1cd
        playingSonar = MonoHelper.Instance.StartCoroutine(SonarSpread());
    }

    private void Counter()
    {
        Debug.Log("Try counter");

        foreach (GameObject otherPlayerObj in PlayerManager.Instance.Players)
        {
            if (otherPlayerObj != parent)
            {
                Sonar enemySonar = otherPlayerObj.GetComponent<Sonar>();
                if (enemySonar.IsSonarOpen)
                {
                    enemySonar.Reset();
                }
            }
            else
            {
                Debug.Log("No one is using sonar???");
            }
        }
        //TODO: -2(?)xcd
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsSonarOpen) return;
        if (other.gameObject.tag == ProjectConst.PlayerTag)
        {

            if (Physics2D.Linecast(PlayerManager.Instance.Players[0].transform.position, PlayerManager.Instance.Players[1].transform.position, detectLayer))
            {
                DomainEvents.Raise(new OnPlayerTrigger(Sonar.SonarState.indirect, parent));
                Debug.Log("Indirect Collide player!!");
            }
            else
            {
                Debug.Log("Direct Collide player!!");
                DomainEvents.Raise(new OnPlayerTrigger(SonarState.direct, parent));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsSonarOpen) return;
        if (other.gameObject.TryGetComponent<Sonar>(out Sonar collideSonar))
        {
            if (collideSonar.IsSonarOpen)
            {
                CancelAllSonar(collideSonar);
            }
        }
    }

    private void CancelAllSonar(Sonar collideSonar)
    {
        Reset();
        collideSonar.Reset();
    }

    private void Reset()
    {
        if (playingSonar != null)
        {
            MonoHelper.Instance.StopCoroutine(playingSonar);
            playingSonar = null;
        }
        SetSonarOpen(false);
        sonarCollider.radius = sonarMinRadius;
        sonar.transform.localScale = Vector3.one;
    }

    IEnumerator SonarSpread()
    {
        SetSonarOpen(true);
        //while (sonarCollider.radius < sonarMaxRadius)
        while (sonar.transform.localScale.x < sonarMaxRadius)
        {
            //sonarCollider.radius += sonarDetectSpeed * Time.deltaTime;
            sonar.transform.localScale += new Vector3(sonarMaskDetectSpeed * Time.deltaTime, sonarMaskDetectSpeed * Time.deltaTime, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(sonarStayDuration);
        Reset();
    }

    private void SetSonarOpen(bool isOpen)
    {
        IsSonarOpen = isOpen;
        gameObject.SetActive(isOpen);
    }

    private void OnDrawGizmos()
    {
        if (sonarCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sonarCollider.radius);
        }
    }
}
