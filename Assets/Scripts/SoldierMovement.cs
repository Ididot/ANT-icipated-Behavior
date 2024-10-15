using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject nest;
    private GameObject enemy;

    public NavMeshHit hit;
    public bool foundEnemy = false;
    public float detectionRange = 5.0f; // Hur nära myran behöver va för att upptäcka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall för random movement
    private float time2NextMove = 0.0f; // Trackar när myrar borde flytta sig härnäst
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Patrol()
    {
        Vector3 randPos = RandPlaneLoc(10.0f);
        agent.SetDestination(randPos);
        // Resetta timern
        time2NextMove = moveCooldown;
    }

    public Vector3 RandPlaneLoc(float range)
    {
        Vector3 randDirection = Random.insideUnitSphere * range;
        // Förskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        // Kollar om random position är på NavMesh och store:ar resultatet i "hit"
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(foundEnemy)
        {
            // Decreasa timern med varje frame
            time2NextMove -= Time.deltaTime;

            // Funktionen kallas bara när myran har nått sin destination och off movement cooldown
            //Här vill vi potentiellt implementera spåren, så dem hittar tillbaka till food
            if(agent.remainingDistance <= agent.stoppingDistance && time2NextMove <= 0.0f)
            {
                Patrol();
            }

            float dist2Food = Vector3.Distance(transform.position, enemy.transform.position);

            // Om mat hittad
            if(dist2Food <= detectionRange)
            {
                // Gå mot mat
                agent.destination = enemy.transform.position;
            }

            // Om myran är framme vid maten
            if(dist2Food <= 1.0f)
            {
                Debug.Log("Time to die");
                foundEnemy = false;
            }
        }
        else
        {
            // Gå mot nest, för att byta tillbaka
            agent.destination = nest.transform.position;

            
        }

        
    }

    //Check collision with food and ant
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Enemy") )
        { 
            
        }
    }
}
