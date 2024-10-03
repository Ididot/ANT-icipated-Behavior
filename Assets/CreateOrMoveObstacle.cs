using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateOrMoveObstacle : MonoBehaviour
{
    public GameObject obstacle;
    private Vector3 StartPoint;
    private Vector3 EndPoint;
    private Vector3 InitialScale;
    private Vector3 currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        InitialScale = transform.localScale;
        Debug.Log(InitialScale);
        UpdateTransformForScale();
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        EndPoint = Input.mousePosition-GetMousePosition();
        
    }

    private void OnMouseDown()
    {
        //StartPoint = Camera.main.(Input.mousePosition);
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(EndPoint);
        Vector3 scaledEndPoint = new Vector3(EndPoint.x, EndPoint.z, EndPoint.y);
        
        //InitialScale.x = InitialScale.x / InitialScale.sqrMagnitude  ;
        InitialScale= InitialScale.normalized;
        scaledEndPoint.Scale(InitialScale);
        Debug.Log(scaledEndPoint);
        Instantiate(obstacle, scaledEndPoint, Quaternion.FromToRotation(StartPoint,EndPoint), transform);
        
        if (Input.GetKey(KeyCode.O))
        {

        }
    }

    private void OnMouseUp()
    {
        //Instantiate(obstacle, Camera.main.ViewportToWorldPoint(EndPoint), Quaternion.FromToRotation(EndPoint, StartPoint), transform);
    }
    void UpdateTransformForScale()
    {
        //float distance = Vector3.Distance()
    }
}
