using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject food;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (food != null)
        {
            Move2Food();
        }
    }

    public void SetFood(GameObject foodObject)
    {
        food = foodObject;
    }

     void Move2RandPos()
    {
        Vector3 randPos = RandPlaneLoc(10.0f);
        agent.SetDestination(randPos);
    } 

    void Move2Food()
    {
        if (food != null)
        {
            // H�mta positionen av le food
            Vector3 foodPosition = food.transform.position;
        }
        else
        {
            // G� runt randomly n�r mat inte �r hittad
            Move2RandPos();
        }
    }

    public Vector3 RandPlaneLoc(float range)
    {
        Vector3 randDirection = Random.insideUnitSphere * range;
        // F�rskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        NavMeshHit hit;
        // Kollar om random position �r p� NavMesh och store:ar resultatet i "hit"
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Avst�nd mellan myra & mat
        float dist2Food = Vector3.Distance(transform.position, food.transform.position);

        if(dist2Food <= 1.0f)
        {
            Move2RandPos(); // Ba placeholder innan myran tar med mat hem �r implementerad
        }

        /* if (count%1000==0)
        {
            Move2RandPos();
            count=0; 
        }
        ++count; */
    }
}
