using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private DroneHandler droneHandler;
    private Vector3 initialPostion;
    private Vector3 finalPosition;
   
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

        // using x100 to convert distance from meters to cm
        if (direction.x<0)
        {
            droneHandler.MoveLeft(ConvertToCentimeters(direction.x));
            
            
            
            // Move left
        }
        else if (direction.x >0)
        {
            droneHandler.MoveRight(ConvertToCentimeters(direction.x));
            // Move Right
        }
        if (direction.y<0)
        {
            // Move Down
            droneHandler.Descend(ConvertToCentimeters(direction.y));
           
        }

        else if (direction.y>0)
        {
            // Move up
            droneHandler.Ascend(ConvertToCentimeters(direction.y));
            

        }

        if (direction.z < 0)
        {
            // Move Back
            droneHandler.MoveBack(ConvertToCentimeters(direction.z));
            
        }

        else if (direction.z > 0)
        {
            // Move forward
            droneHandler.MoveForward(ConvertToCentimeters(direction.z));
            


        }


    }

    private static Vector3 Distance(Vector3 initialPostion, Vector3 finalPosition)
    {
        Vector3 distiance = finalPosition - initialPostion;

        return distiance;

    }

    private static float ConvertToCentimeters(float value)
    {
        return Mathf.Abs(value * 100f);
    }

}
