using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject food;
<<<<<<< Updated upstream
    private int count = 0;
=======
    public GameObject bbyFood;
    public bool foundFood = false;
    public float detectionRange = 5.0f; // Hur nära myran behöver va för att upptäcka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall för random movement
    private float time2NextMove = 0.0f; // Trackar när myrar borde flytta sig härnäst
    public GameObject child;
    public bool hasChild = false;
    private enum AntState {
        Idle,
        Search,
        Gather,
        Return,
        Attacked
        };
    private AntState currentState = AntState.Idle;

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
<<<<<<< Updated upstream

        if (food != null)
        {
            Move2Food();
        }
=======
        Transition2State(AntState.Search);
>>>>>>> Stashed changes
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
            // Hämta positionen av le food
            Vector3 foodPosition = food.transform.position;
        }
        else
        {
            // Gå runt randomly när mat inte är hittad
            Move2RandPos();
        }
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

    void Transition2State(AntState newState)
    {
        currentState = newState;
        Debug.Log("TRANSITIONED TO STATE: " + newState.ToString());
    }

    void Search4Food()
    {
        // Myrorna förflyttar sig randomly för att söka efter mat
        time2NextMove -= Time.deltaTime;
        if (time2NextMove <= 0.0f && agent.remainingDistance <= agent.stoppingDistance)
        {
            Move2RandPos();
            time2NextMove = moveCooldown;
        }

        // Om mat är inom detection range, gå dit och sen byt till Gather state
        float dist2Food = Vector3.Distance(transform.position, food.transform.position);

        if (food != null && dist2Food <= detectionRange)
        {
            agent.destination = food.transform.position;
            Transition2State(AntState.Gather);
        }
    }

    void GatherFood()
    {
        float dist2Food = Vector3.Distance(transform.position, food.transform.position);

        if(dist2Food <= 1.0f)
        {
            agent.destination = nest.transform.position;
            // Om myran är framme vid maten
            if(dist2Food <= 1.0f)
            {
                foundFood = true;
                Debug.Log("Wooow foowmd!!");
            }
            Transition2State(AntState.Return);
        }
    }

    void Return2Nest()
    {
        float dist2Nest = Vector3.Distance(transform.position, nest.transform.position);
        
        if(dist2Nest <= 1.0f)
        {
            Debug.Log("Food deposited.");
            Transition2State(AntState.Search);
        }
    }

    void HandleAttack() 
    {
        Destroy(gameObject);
    }

    public void Attacked()
    {
        Transition2State(AntState.Attacked);
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        // Avstånd mellan myra & mat
        float dist2Food = Vector3.Distance(transform.position, food.transform.position);

        if(dist2Food <= 1.0f)
=======
        switch(currentState)
        {
            case AntState.Idle:
                // Gör nada
                break;
            
            case AntState.Search:
                Search4Food();
                break;
            
            case AntState.Gather:
                GatherFood();
                break;
            
            case AntState.Return:
                Return2Nest();
                break;

            case AntState.Attacked:
                HandleAttack();
                break;
        }

        // *** NEDAN FÖLJER HELA GAMLA UPDATE-LOGIKEN UTAN SWITCH-CASE ***
        /* if(!foundFood&&!hasChild)
>>>>>>> Stashed changes
        {
            Move2RandPos(); // Ba placeholder innan myran tar med mat hem är implementerad
        }

        /* if (count%1000==0)
        {
            Move2RandPos();
            count=0; 
        }
<<<<<<< Updated upstream
        ++count; */
=======
        ++count;

        if(hasChild && child != null) //Check if ant has bbyfood, if it do track the food to ant position
        child.transform.position = new Vector3(transform.position.x,transform.position.y+0.5f, transform.position.z); */
    }

    //Check collision with food and ant
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Food")&&!hasChild)//Ensure collision is actually food
        {
            other.gameObject.GetComponent<foodManager>().updateFoodStash();
            //Create bbyfood
            child=Instantiate(bbyFood, transform.position, other.gameObject.transform.rotation, transform);
            hasChild = true;
            
        }
>>>>>>> Stashed changes
    }
}
