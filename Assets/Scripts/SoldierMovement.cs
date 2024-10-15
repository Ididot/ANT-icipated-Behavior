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
    public float detectionRange = 5.0f; // Hur n�ra myran beh�ver va f�r att uppt�cka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall f�r random movement
    private float time2NextMove = 0.0f; // Trackar n�r myrar borde flytta sig h�rn�st
    

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
        // F�rskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        // Kollar om random position �r p� NavMesh och store:ar resultatet i "hit"
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

            // Funktionen kallas bara n�r myran har n�tt sin destination och off movement cooldown
            //H�r vill vi potentiellt implementera sp�ren, s� dem hittar tillbaka till food
            if(agent.remainingDistance <= agent.stoppingDistance && time2NextMove <= 0.0f)
            {
                Patrol();
            }

            float dist2Food = Vector3.Distance(transform.position, enemy.transform.position);

            // Om mat hittad
            if(dist2Food <= detectionRange)
            {
                // G� mot mat
                agent.destination = enemy.transform.position;
            }

            // Om myran �r framme vid maten
            if(dist2Food <= 1.0f)
            {
                Debug.Log("Time to die");
                foundEnemy = false;
            }
        }
        else
        {
            // G� mot nest, f�r att byta tillbaka
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
