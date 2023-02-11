using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private DroneHandler droneHandler;
    private Vector3 initialPostion;
    private Vector3 finalPosition;
   
    private void Update()
    {
        Vector3 objectPosition = cube.transform.position;
        



        //Debug.Log(objectPosition);
        //Debug.Log(objectphysics.velocity);

    }
    public void IsGrabbed() 
    {
        initialPostion = cube.transform.position;
        Debug.Log("Cube is grabbed");

    }
    public void IsReleased()
    {
        finalPosition = cube.transform.position;
        Vector3 distance = Distance(initialPostion, finalPosition);
        InitiateMovement(distance);

        Debug.Log("Cube is released");
    }

    private void InitiateMovement(Vector3 direction)
    {
        if (direction.x<0)
        {
            droneHandler.MoveLeft(Mathf.Abs(direction.x) * 100);
            
            // Move left
        }
        else if (direction.x >0)
        {
            droneHandler.MoveRight(Mathf.Abs(direction.x)*100);
            // Move Right
        }
        if (direction.y<0)
        {
            // Move Down
        }

        else if (direction.y>0)
        {
            // Move up

        } 


    }

    private static Vector3 Distance(Vector3 initialPostion, Vector3 finalPosition) 
    {
        Vector3 distiance = finalPosition - initialPostion;

        return distiance;
    
    } 

}
