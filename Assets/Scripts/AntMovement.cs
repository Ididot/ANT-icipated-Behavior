using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject nest;
    //private int count = 0;
    public NavMeshHit hit;
    public GameObject food;
    public GameObject bbyFood;
    public bool foundFood = false;
    public float detectionRange = 5.0f; // Hur n�ra myran beh�ver va f�r att uppt�cka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall f�r random movement
    private float time2NextMove = 0.0f; // Trackar n�r myrar borde flytta sig h�rn�st
    public GameObject child;
    public bool hasChild = false;

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
        // F�rskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        // Kollar om random position �r p� NavMesh och store:ar resultatet i "hit"
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!foundFood&&!hasChild)
        {
            // Decreasa timern med varje frame
            time2NextMove -= Time.deltaTime;

            // Funktionen kallas bara n�r myran har n�tt sin destination och off movement cooldown
            //H�r vill vi potentiellt implementera sp�ren, s� dem hittar tillbaka till food
            if(agent.remainingDistance <= agent.stoppingDistance && time2NextMove <= 0.0f &&!hasChild)
            {
                Move2RandPos();
            }

            float dist2Food = Vector3.Distance(transform.position, food.transform.position);

            // Om mat hittad
            if(dist2Food <= detectionRange)
            {
                // G� mot mat
                agent.destination = food.transform.position;
            }

            // Om myran �r framme vid maten
            if(dist2Food <= 1.0f)
            {
                foundFood = true;
                //Debug.Log("Wooow foowmd!!");
            }
        }
        else
        {
            float dist2Nest = Vector3.Distance(transform.position, nest.transform.position);
            // G� mot nest
            agent.destination = nest.transform.position;

            // Om myran �r framme vid nest
            if(dist2Nest <= 1.0f)
            {
                // L�mna ifr�n maten
                foundFood = false;
                //Debug.Log("Time 4 chomk! yummy");
                //Found if storage allows more food, accept food remove food from ant and spawn new ant
                if(nest.GetComponent<Nest>().food< nest.GetComponent<Nest>().foodMax)
                {
                    //Destroy, this objects(Ants) first child
                    Destroy(gameObject.transform.GetChild(0).gameObject);
                    hasChild = false;
                    nest.GetComponent<Nest>().spawnAnt(); //Funktion f�r att instantiera nya myror
                    ++nest.GetComponent<Nest>().food;
                }
                
                
                // Forts�tt leta efter mer mat
                Move2RandPos();
            }
        }

        // G�r dirr till maten
        //agent.destination = food.transform.position;

        /* if (count%1000==0)
        {
            Move2RandPos();
            count=0; 
        }
        ++count; */

        if(hasChild) //Check if ant has bbyfood, if it do track the food to ant position
        child.transform.position = new Vector3(transform.position.x,transform.position.y+0.5f, transform.position.z);
    }

    //Check collision with food and ant
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Food")&&!hasChild)//Ensure collision is actually food
        {
            //Create bbyfood
            child=Instantiate(bbyFood, transform.position, other.gameObject.transform.rotation, transform);
            hasChild = true;
            
        }
    }
}
