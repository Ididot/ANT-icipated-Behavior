using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodResource : MonoBehaviour
{

    //Size of the food -> 5/10/20 pieces of food
    //Which ants back will the food piece ride on
    public GameObject Ant;
    private Vector3 babyFoodTransCoef;//Small icon for ants transform coefficient
    private class size
    {
        public int index = 0;
        public int pieces = 0;
        public float transformCoef=1.0f;

        public size( int _size)
        {
            index = _size;

            if (_size == 1)
            {
                pieces = 5;
                transformCoef = 0.5f;
            }
            if (_size == 2)
            {
                pieces = 10;
                transformCoef=1.0f;
            }
            if(_size == 3)
            {
                pieces = 20;
                transformCoef=1.5f;
            }
            else { Debug.Log("Faulty Size, for food"); }
        }
        
    }
    
    



    // Start is called before the first frame update
    void Start()
    {
        babyFoodTransCoef = 0.1f * Ant.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
