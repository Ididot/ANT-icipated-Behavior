using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject nest;
    public GameObject food;
    public GameObject bbyFood;
    public bool foundFood = false;
    public float detectionRange = 1.25f; // Hur nära myran behöver va för att upptäcka att det finns mat
    public float trailDetectionRange = 5.0f;
    public float enemyDetectionRange = 3.5f;
    private Vector3 initialFoodPosition;
    //public float moveCooldown = 2.0f; // Tidsintervall för random movement
    //private float time2NextMove = 0.0f; // Trackar när myrar borde flytta sig härnäst
    private GameObject child;
    public bool hasChild = false;
    private Vector3[] trailPositions;
    public enum AntState
    {
        Idle,
        Search,
        Return,
        Flee,
        Attacked
    };
    [SerializeField] private AntState currentState = AntState.Idle;
    private TrailManager trailManager;
    private Vector3 lastTrailPosition;


    // Start is called before the first frame update
    void Start()
    {
        child = bbyFood;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 4.0f;
        trailManager = FindObjectOfType<TrailManager>();
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
        // Förskjuter riktningen relativt till myrans position
        randDirection += transform.position;

        NavMeshHit hit;
        // Kollar om random position är på NavMesh och store:ar resultatet i "hit"
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    public void Transition2State(AntState newState)
    {
        currentState = newState;
       // Debug.Log("TRANSITIONED TO STATE: " + newState.ToString());
    }

    void Search4Food()
    {
        if (hasChild)
        {
            Transition2State(AntState.Return);
            return; // Gå ur tidigt pga den borde gå tbx till nest
        }

        // Myrorna förflyttar sig randomly för att söka efter mat
        if (!foundFood && !hasChild)
        {
            // Kolla efter trails i närheten
            Vector3 trailPoint = trailManager.GetClosestTrailPoint(transform.position, trailDetectionRange);
            if (trailPoint != Vector3.zero)
            {
                // Följ trailen
                agent.destination = lastTrailPosition;
                lastTrailPosition = trailPoint; // Spara positionen som en referens
            }
            else
            {
                // Ingen trail hittad, gå runt randomly
                agent.stoppingDistance = 0.2f;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Move2RandPos();
                }
            }

            // Om mat är inom detection range, gå dit och sen byt till Return state           
            Collider[] foodColliders = Physics.OverlapSphere(transform.position, detectionRange);
            foreach (Collider foodCollider in foodColliders)
            {
                if (foodCollider.CompareTag("Food") && foodCollider.transform.parent == null)
                {
                    food = foodCollider.gameObject; // Denna behövs för att uppdatera matreferensen, som nu är upplockad
                    initialFoodPosition = food.transform.position;
                    agent.destination = initialFoodPosition;

                    float dist2Food = Vector3.Distance(transform.position, food.transform.position);
                    if (dist2Food <1.0f) 
                    {
                        foundFood = true;
                        //Debug.Log("Wooow foowmd!!");
                        Transition2State(AntState.Return);
                    }
                }
            }
        }
        else if (!hasChild)
        {
            Transition2State(AntState.Search);
            foundFood = true;
        }
    }

    void GatherFood()
    {
        if (food != null)
        {
            float dist2Food = Vector3.Distance(transform.position, food.transform.position);

            if (dist2Food <= 1.0f)
            {
                foundFood = true;
                //Debug.Log("Wooow foowmd!!");
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
        if (hasChild) //Ant has food to return to nest with
        {
            agent.destination = nest.transform.position;
            GetComponent<TrailRenderer>().emitting = true;
            trailManager.AddTrailPoint(transform.position);

            float dist2Nest = Vector3.Distance(transform.position, nest.transform.position);
            if (dist2Nest <= 1.0f)
            {
                foundFood = false;
                //Debug.Log("Food deposited.");
                food = null;

                //Found if storage allows more food, accept food remove food from ant and spawn new ant
                if (nest.GetComponent<Nest>().food < nest.GetComponent<Nest>().foodMax)
                {
                    if (transform.childCount > 0) // Ensure the ant has a child before attempting to destroy it
                    {
                        Destroy(gameObject.transform.GetChild(0).gameObject); // Destroy the first child
                        nest.GetComponent<Nest>().spawnAnt(this); // Spawn a new ant
                        nest.GetComponent<Nest>().food++;
                    }
                }
                
                GetComponent<TrailRenderer>().emitting = false;
                Transition2State(AntState.Search);
                /*Vector3 temp=GetComponent<TrailRenderer>().GetPosition(GetComponent<TrailRenderer>().positionCount-1);
                Debug.Log(temp + "<-Trail end / ANT position->"+transform.position ); */

            }
            //int posCount = GetComponent<TrailRenderer>().GetVisiblePositions(trailPositions);
        }
        else
        {
            Transition2State(AntState.Search);
        }
    }

    void FleeFromEnemy()
    {
        if (hasChild)
        {
            Transition2State(AntState.Return);
        }
        Collider[] enemyInRange = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider enemy in enemyInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Vector3 fleeDirection = (transform.position - enemy.transform.position).normalized;
                Vector3 fleeDestination = transform.position + fleeDirection * detectionRange;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(fleeDestination, out hit, detectionRange, 1))
                {
                    agent.destination = hit.position;
                }
                return;
            }
        }
        Transition2State(AntState.Search) ;
    }

    void HandleAttack()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    public void Attacked()
    {
        Transition2State(AntState.Attacked);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AntState.Idle:
                // Gör nada
                break;

            case AntState.Search:
                Search4Food();
                break;

            case AntState.Return:
                Return2Nest();
                break;

            case AntState.Flee:
                // Behöver inte kalla på den igen, ba fortsätt fly
                FleeFromEnemy();
                break;

            case AntState.Attacked:
                HandleAttack();
                break;
        }

        if (hasChild && child != null) //Check if ant has bbyfood, if it do track the food to ant position
            child.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    //Check collision with food and ant
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food") && !hasChild)//Ensure collision is actually food
        {
            other.gameObject.GetComponent<foodManager>().updateFoodStash();

            //Create bbyfood
            child = Instantiate(bbyFood, transform.position, other.gameObject.transform.rotation, transform);
            hasChild = true;
            food = null;
        }
    }
}
