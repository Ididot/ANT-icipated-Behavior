using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private int count = 0;
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
    }

    public Vector3 RandPlaneLoc(float range)
    {
        Vector3 randDirection = Random.insideUnitSphere * range;
        // Förskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        NavMeshHit hit;
        // Kollar om random position är på NavMesh och store:ar resultatet i "hit"
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (count%1000==0)
        {
            Move2RandPos();
            count=0; 
        }
        ++count;
        
        
    }
}
