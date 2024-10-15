using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using foodSize;

public class foodManager : MonoBehaviour
{
    public size foodSizeforFood;
    [SerializeField] int foodCount;
    [SerializeField] int foodMaximum;

    void Start()
    {
        foodCount = foodSizeforFood.activePieces;
        foodMaximum = foodSizeforFood.maxPieces;

    }
    public void updateFoodStash()
    {
        if (foodCount < foodMaximum) 
        ++foodCount;
        
            

        if (foodCount== foodMaximum)
        {
            Destroy(gameObject);
        }
    }
}
