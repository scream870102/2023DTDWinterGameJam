using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject sonarPrefab;
    [SerializeField]private float sonarMaxRadius = 3;
    [SerializeField]private float sonardetectSpeed = 0.1f;

    private CircleCollider2D sonarCollider;

    void Start()
    {
        InitSonar();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("sonar appear");
            SonarDetect();
        }
    }

    private void InitSonar()
    {
        GameObject sonar = Instantiate(sonarPrefab, this.transform);
        sonarCollider = sonar.AddComponent<CircleCollider2D>();
    }

    private void SonarDetect()
    {
        if(sonarCollider==null) return;
        
        StartCoroutine(SonarSpread());
        return;

        
    }

    IEnumerator SonarSpread()
    {
        while(sonarCollider.radius < sonarMaxRadius)
        {
            sonarCollider.radius += sonardetectSpeed;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("sonar max reach");
        yield return new WaitForSeconds(2f);
        sonarCollider.radius = 0.5f;

        //return null;
    }

    private void OnDrawGizmos() {
        if(sonarCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sonarCollider.radius);
        }
    }
}
