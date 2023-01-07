using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scream.UniMO;
using Scream.UniMO.Common;
using System;

public class OnPlayerTrigger : IDomainEvent
{
    public Sonar.SonarState State{get; private set;}
    public GameObject Player{get; private set;}

    public OnPlayerTrigger(Sonar.SonarState state, GameObject player)
    {
        State = state;
        Player = player;
    }
}

public class Sonar : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Player{
        player1, player2
    }

    public GameObject anotherPlayer;
    public enum SonarState{
        direct, indirect
    }
    public Player player;
    [SerializeField]private GameObject sonarPrefab;
    [SerializeField]private float sonarMinRadius = 0.5f;
    [SerializeField]private float sonarMaxRadius = 3;
    [SerializeField]private float sonarDetectSpeed = 0.1f;
    [SerializeField]private float sonarStayDuration = 0.5f;
    [SerializeField]private float sonarMaskDetectSpeed = 0.5f;

    private GameObject sonar;
    private CircleCollider2D sonarCollider;
    private bool isSonarOpen = false;
    public bool IsSonarOpen => isSonarOpen;

    private void Awake() {
        //DomainEvents.Register<onPlayerAction>(onPlayerActionEvent);
    }

    private void Start()
    {
        InitSonar();
    }

    private void OnDestroy() {
        //DomainEvents.UnRegister<onPlayerAction>(onPlayerActionEvent);    
    }

    // Update is called once per frame
    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.K) && player == Player.player1)
        // {
        //     Detect();
        // }
        // if(Input.GetKeyDown(KeyCode.S) && player == Player.player2)
        // {
        //     Detect();
        // }
        // if(Input.GetKeyDown(KeyCode.L) && player == Player.player1)
        // {
        //     Counter();
        // }
        // if(Input.GetKeyDown(KeyCode.A) && player == Player.player2)
        // {
        //     Counter();
        // }
    }

    private void OnPlayerTriggerEvent(OnPlayerTrigger param){ }

    

    private void InitSonar()
    {
        sonar = Instantiate(sonarPrefab, this.transform);
        sonarCollider = this.gameObject.AddComponent<CircleCollider2D>();
        SpriteMask sonarMask = sonar.GetComponent<SpriteMask>();
        sonarCollider.isTrigger = true;
    }

    private void Detect()
    {
        if(sonarCollider==null) return;
        if(isSonarOpen) return;
        // -1cd
        StartCoroutine(SonarSpread());
        return; 
    }

    private void Counter()
    {
        Debug.Log("Try counter");
        for(int i=0;i<PlayerManager.Instance.Players.Length;i++)
        {
            if(PlayerManager.Instance.Players[i] != gameObject)
            {
                Sonar enemySonar = PlayerManager.Instance.Players[i].GetComponent<Sonar>();
                if(enemySonar.isSonarOpen)
                {
                    enemySonar.Reset();
                }
            }
            else{
                Debug.Log("No one is using sonar???");
            }
        }
        // -2(?)xcd
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!isSonarOpen) return;
        if(other.gameObject.tag == "playertest")
        {
            
            if(Physics2D.Linecast(PlayerManager.Instance.Players[0].transform.position,PlayerManager.Instance.Players[1].transform.position))
            {
                DomainEvents.Raise(new OnPlayerTrigger(Sonar.SonarState.indirect,this.gameObject));
                Debug.Log("Indirect Collide player!!");
            }
            else
            {
                Debug.Log("Direct Collide player!!");
                DomainEvents.Raise(new OnPlayerTrigger(SonarState.direct,this.gameObject));
            }            
        }
    }

    
    private void OnTriggerStay2D(Collider2D other)    
    {
        if(!isSonarOpen) return;
        if(other.gameObject.TryGetComponent<Sonar>(out Sonar collideSonar))
        {
            if(collideSonar.IsSonarOpen)
            {
                CancelAllSonar(collideSonar);
            }
        }
    }

    private void CancelAllSonar(Sonar collideSonar)
    {
        this.Reset();
        collideSonar.Reset();
    }

    private void Reset() {
        StopAllCoroutines();
        isSonarOpen = false;
        sonarCollider.radius = sonarMinRadius;
        sonar.transform.localScale = new Vector3(1,1,1);

    }

    IEnumerator SonarSpread()
    {
        isSonarOpen = true;
        while(sonarCollider.radius < sonarMaxRadius)
        {
            sonarCollider.radius += sonarDetectSpeed;
            sonar.transform.localScale += new Vector3(sonarMaskDetectSpeed,sonarMaskDetectSpeed,1);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(sonarStayDuration);
        Reset();
    }

    private void OnDrawGizmos() {
        if(sonarCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sonarCollider.radius);
        }
    }
}
