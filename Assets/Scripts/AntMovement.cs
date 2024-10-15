using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject nest;
    public GameObject food;
    public GameObject bbyFood;
    public bool foundFood = false;
    public float detectionRange = 5.0f; // Hur n�ra myran beh�ver va f�r att uppt�cka att det finns mat
    public float moveCooldown = 2.0f; // Tidsintervall f�r random movement
    private float time2NextMove = 0.0f; // Trackar n�r myrar borde flytta sig h�rn�st
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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Transition2State(AntState.Search);
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

    void Transition2State(AntState newState)
    {
        currentState = newState;
        Debug.Log("TRANSITIONED TO STATE: " + newState.ToString());
    }

    void Search4Food()
    {
        // Myrorna f�rflyttar sig randomly f�r att s�ka efter mat
        
        if(!foundFood && !hasChild)
        {
            time2NextMove -= Time.deltaTime;

            if (agent.remainingDistance <= agent.stoppingDistance && time2NextMove <= 0.0f)
            {
                Move2RandPos();
                time2NextMove = moveCooldown;
            }

            // Om mat �r inom detection range, g� dit och sen byt till Gather state           
            Collider[] foodColliders = Physics.OverlapSphere(transform.position, detectionRange);
            foreach(Collider foodCollider in foodColliders)
            {
                if(foodCollider.CompareTag("Food") && foodCollider.transform.parent == null)
                {
                    food = foodCollider.gameObject; // Denna beh�vs f�r att uppdatera matreferensen, som nu �r upplockad
                    agent.destination = food.transform.position;
                    Transition2State(AntState.Gather);
                    return;
                }
            }
            
            /*
            float dist2Food = Vector3.Distance(transform.position, food.transform.position);

            if (food != null && dist2Food <= detectionRange)
            {
                agent.destination = food.transform.position;
                Transition2State(AntState.Gather);
            }
            */
        }
    }

    void GatherFood()
    {
        if(food != null)
        {
            float dist2Food = Vector3.Distance(transform.position, food.transform.position);

            if(dist2Food <= 1.0f)
            {
                foundFood = true;
                Debug.Log("Wooow foowmd!!");
                Transition2State(AntState.Return);
            }
        }
        else
        {
            Transition2State(AntState.Search);
        }
    }

    void Return2Nest()
    {    
        if(foundFood)
        {
            agent.destination = nest.transform.position;

            float dist2Nest = Vector3.Distance(transform.position, nest.transform.position);
            if(dist2Nest <= 1.0f)
            {
                foundFood = false;
                Debug.Log("Food deposited.");
                food = null;

                //Found if storage allows more food, accept food remove food from ant and spawn new ant
                if (nest.GetComponent<Nest>().food < nest.GetComponent<Nest>().foodMax)
                {
                    if (transform.childCount > 0) // Ensure the ant has a child before attempting to destroy it
                    {
                        if(gameObject.transform.GetChild(0) != null)
                        {
                            Destroy(gameObject.transform.GetChild(0).gameObject); // Destroy the first child
                        }
                        hasChild = false;
                        nest.GetComponent<Nest>().spawnAnt(); // Spawn a new ant
                        nest.GetComponent<Nest>().food++;
                    }
                }
                Transition2State(AntState.Search);
            }
        }
        else
        {
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
        switch(currentState)
        {
            case AntState.Idle:
                // G�r nada
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

        if(hasChild && child != null) //Check if ant has bbyfood, if it do track the food to ant position
        child.transform.position = new Vector3(transform.position.x,transform.position.y+0.5f, transform.position.z);

        // *** NEDAN F�LJER HELA GAMLA UPDATE-LOGIKEN UTAN SWITCH-CASE ***
        /* if(!foundFood&&!hasChild)
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
                if (nest.GetComponent<Nest>().food < nest.GetComponent<Nest>().foodMax)
                {
                    if (transform.childCount > 0) // Ensure the ant has a child before attempting to destroy it
                    {
                        Destroy(gameObject.transform.GetChild(0).gameObject); // Destroy the first child
                        hasChild = false;
                        nest.GetComponent<Nest>().spawnAnt(); // Spawn a new ant
                        ++nest.GetComponent<Nest>().food;
                    }
                }
                // Forts�tt leta efter mer mat
                Move2RandPos();
            }
        } 
        */
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
            food = null;
        }
    }
}