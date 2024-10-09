using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateObstacle : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject food;
    private Vector3 worldPos;
    

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
            Instantiate(obstacle, worldPos,Quaternion.identity);
            
        }
        else
        {
            Instantiate(food, worldPos, food.transform.localRotation);
        }
    }

    private void OnMouseUp()
    {
        
    }
}
