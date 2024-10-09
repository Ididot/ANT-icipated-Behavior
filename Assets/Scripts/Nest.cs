using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public GameObject AntBasic;
    //Public values of nest
    public float Energy;
    public int AntCountMax;
    public int AntCount;
    public int food;
    public int foodMax;
    private Vector3 spawnPos;

    private class AntClasses
    { //Default Ant-class
        int AntHealth;
        int spawnEnergy;
        float movementCost;
        int carryCapacity;

    }
    

    // Start is called before the first frame update
    void Start()
    {
        spawnPos.x = transform.position.x;
        spawnPos.y = AntBasic.transform.position.y;//Same height as the ant prefab
        spawnPos.z = transform.position.z;

        Energy = 100f;
        AntCountMax = 100; //Nest can hold a maximum of 100 ants on new game start.
        AntCount = 10; //You start with 10 ants
        food = 0;
        foodMax = 50;
        //Spawn start amount of Ants
        for (int i = 0; i < AntCount-1; i++) //-1 Eftersom första myran återanvänds
        {
            Instantiate(AntBasic, spawnPos, transform.rotation, transform);
        }
        
    }

    public void spawnAnt()
    {
        if (AntCount < AntCountMax)
        {
        Instantiate(AntBasic, spawnPos, transform.rotation, transform);
        ++AntCount;

        }

    }

    // Update is called once per frame
    void Update()
    {
        Energy -= 0.001f;
    }
}
