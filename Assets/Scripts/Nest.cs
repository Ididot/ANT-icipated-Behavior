using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public GameObject AntBasic;
    public GameObject SoldierAnt;
    private GameObject _spawnAnt;
    private GameObject _enemy;
    //Public values of nest
    public float Energy;
    public int AntCountMax;
    public int AntCount;
    public int food;
    public int foodMax;
    private Vector3 spawnPos;

    public bool changeClass = false;
    public bool threatDet = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnAnt = AntBasic; //Save gameobject in-case GO is killed first
        spawnPos.x = transform.position.x;
        spawnPos.y = _spawnAnt.transform.position.y;//Same height as the ant prefab
        spawnPos.z = transform.position.z;


        Energy = 100f;
        AntCountMax = 100; //Nest can hold a maximum of 100 ants on new game start.
        AntCount = 10; //You start with 10 ants
        food = 0;
        foodMax = 50;
        //Spawn start amount of Ants
        for (int i = 0; i < AntCount-1; i++) //-1 Eftersom första myran återanvänds
        {
            Instantiate(_spawnAnt, spawnPos, transform.rotation, transform);
        }
        
    }

    public void spawnAnt(AntMovement ant)
    {
        if (AntCount < AntCountMax && ant.hasChild)
        {
        Instantiate(_spawnAnt, spawnPos, transform.rotation, transform);
        ++AntCount;
            --food;

        }


    }
    public void rememberEnemy(EnemyMovement enemy)
    {
        _enemy = enemy.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //If a threat has been detected, allow ants to swap classes
        if (threatDet)
        {
            changeClass = true;
        }


        Energy -= 0.001f;
    }

    //Functioning collision detection with nest
    private void OnTriggerEnter(Collider other)
    {
        //Create soldier ant
        //if (other.gameObject.CompareTag("Ant")&&changeClass)
        //{

        //    Instantiate(SoldierAnt);
        //    Destroy(other.gameObject);
        //}
        
        /*
        if (other.gameObject.GetComponent<AntMovement>().hasChild)
        {
            Debug.Log("Has bebe?");
            if (other.gameObject.transform.GetChild(0).gameObject == null)
            {
                Debug.Log("Thanks for the bebe");
            }

        }
        */
    }
}
