using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public float detectionRange = 5.0f;
    public float moveCooldown = 3.0f;
    private float time2NextMove = 0.0f;
    public int attackDamage = 1;
    public float attackRange = 0.75f;
    public float attackCooldown = 2.0f;
    private float time2NextAttack = 0.0f;
    //public GameObject nest; // Om enemy ska direkt påverka nest på nåt sätt
    private GameObject targetAnt;

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
        time2NextMove = moveCooldown;
    }

    public Vector3 RandPlaneLoc(float range)
    {
        Vector3 randDirection = Random.insideUnitSphere * range;
        randDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randDirection, out hit, range, 1);
        return hit.position;
    }

    // Update is called once per frame
    void Update()
{
    time2NextMove -= Time.deltaTime;
    time2NextAttack -= Time.deltaTime;

    // Vandra runt randomly om ingen myra hittas
    if (targetAnt == null)
    {
        if (time2NextMove <= 0.0f && agent.remainingDistance <= agent.stoppingDistance)
        {
            Move2RandPos();
        }
    }
    else
    {
        if (targetAnt != null)
        {
            agent.SetDestination(targetAnt.transform.position);
            float distanceToAnt = Vector3.Distance(transform.position, targetAnt.transform.position);

            // Kolla om myran är inom atk range
            if (distanceToAnt <= attackRange && time2NextAttack <= 0.0f)
            {
                AttackAnt(targetAnt);
                time2NextAttack = attackCooldown;
            }
        }
    }
    DetectAnts();
}

void AttackAnt(GameObject ant)
{
    if (ant != null)
    {
        AntMovement antMovement = ant.GetComponent<AntMovement>();

        if (antMovement != null)
        {
            ant.GetComponentInParent<Nest>().AntCount--;
            ant.GetComponentInParent<Nest>().threatDet = true;
            Destroy(ant);
            //nest.GetComponent<Nest>().Energy -= attackDamage;
            // Vill ba ha koden ovan som referens så vi vet hur man kan göra, t.ex. ifall man påverkar nest direkt genom att döda
            Debug.Log("Obliterated! R.I.P. mannen");
            targetAnt = null;
        }
    }
}

    void DetectAnts()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Ant"))
            {
                // Här vill vi bara tracka den första myran som hittas
                targetAnt = hitCollider.gameObject;
                break;
            }
        }
    }
}
