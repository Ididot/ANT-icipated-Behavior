using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject nest;
    private int count = 0;
    public NavMeshHit hit;
    public GameObject food;
    private bool foundFood = false;
    public float detectionRange = 5.0f; // Hur nära myran behöver va för att upptäcka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall för random movement
    private float time2NextMove = 0.0f; // Trackar när myrar borde flytta sig härnäst

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Move2RandPos();
    }

    void Move2RandPos()
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
        if(!foundFood)
        {
            // Decreasa timern med varje frame
            time2NextMove -= Time.deltaTime;

            // Funktionen kallas bara när myran har nått sin destination och off movement cooldown
            if(agent.remainingDistance <= agent.stoppingDistance && time2NextMove <= 0.0f)
            {
                Move2RandPos();
            }

            float dist2Food = Vector3.Distance(transform.position, food.transform.position);

            // Om mat hittad
            if(dist2Food <= detectionRange)
            {
                // Gå mot mat
                agent.destination = food.transform.position;
            }

            // Om myran är framme vid maten
            if(dist2Food <= 1.0f)
            {
                foundFood = true;
                Debug.Log("Wooow foowmd!!");
            }
        }
        else
        {
            float dist2Nest = Vector3.Distance(transform.position, nest.transform.position);
            // Gå mot nest
            agent.destination = nest.transform.position;

            // Om myran är framme vid nest
            if(dist2Nest <= 1.0f)
            {
                // Lämna ifrån maten
                foundFood = false;
                Debug.Log("Time 4 chomk! yummy");
                // Fortsätt leta efter mer mat
                Move2RandPos();
            }
        }

        // Går dirr till maten
        //agent.destination = food.transform.position;

        /* if (count%1000==0)
        {
            Move2RandPos();
            count=0; 
        }
        ++count; */
    }
}
