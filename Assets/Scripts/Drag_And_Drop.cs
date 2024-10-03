using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_And_Drop : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject food;
    Vector3 mousePosition;
    
    private Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePosition();
    }


    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition-mousePosition);
  
    }

}
