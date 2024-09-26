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
        Energy = 100f;
        AntCountMax = 100; //Nest can hold a maximum of 100 ants on new game start.
        AntCount = 10; //You start with 10 ants
        for (int i = 0; i < AntCount; i++)
        {
            Instantiate(AntBasic);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Energy -= 0.001f;
    }
}
