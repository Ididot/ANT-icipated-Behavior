using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using foodSize;

public class CreateObstacle : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject food;
    private Vector3 worldPos;


    //Size of the food -> 5/10/20 pieces of food
    //Which ants back will the food piece ride on
    public GameObject temp;
    public size foodsize;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        
        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.y = 0.5f;


        if (Input.GetKey(KeyCode.O))
        {
            Instantiate(obstacle, worldPos, Quaternion.identity);

        }
        else
        {
            
            foodsize = new size();
            //Create a piece of food
            temp = Instantiate(food, worldPos, food.transform.localRotation);
            //Add a foodManager to the food
            temp.AddComponent<foodManager>();
            //Assign the foodManager the same size as the current food
            temp.GetComponent<foodManager>().foodSizeforFood = foodsize;
            //Transform the food to fit the size for the specific foodSize
            temp.transform.localScale *= foodsize.transformCoef;
        }
    }

}
