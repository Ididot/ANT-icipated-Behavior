using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateObstacle : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject food;
    private Vector3 worldPos;

    //Size of the food -> 5/10/20 pieces of food
    //Which ants back will the food piece ride on
    private class size
    {
        public int index = 0;
        public int pieces = 0;
        public float transformCoef = 1.0f;

        public size(int _size)
        {
            index = _size;

            switch (_size)
            {
                case 1:
                    {
                        pieces = 5;
                        transformCoef = 0.5f;
                        break;
                    }

                case 2:
                    {
                        pieces = 10;
                        transformCoef = 1.0f;
                        break;
                    }
                case 3:
                    {
                        pieces = 20;
                        transformCoef = 1.5f;
                        break;
                    }
                default:
                    {
                        Debug.Log("Faulty Size, for food");
                        break;

                    }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        GameObject temp;
        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.y = 0.5f;


        if (Input.GetKey(KeyCode.O))
        {
            Instantiate(obstacle, worldPos, Quaternion.identity);

        }
        else
        {
            size foodsize = new size((Random.Range(1, 4)));
            temp=Instantiate(food, worldPos, food.transform.localRotation);
            temp.transform.localScale *= foodsize.transformCoef;
        }
    }

    private void OnMouseUp()
    {

    }
}
