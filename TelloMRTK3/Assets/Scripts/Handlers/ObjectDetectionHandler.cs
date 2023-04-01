using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionHandler : MonoBehaviour
{
    public GameObject anchor;
    public ObjectManipulator manipulator;
 

    private void IsAnchored()
    {
        Vector3 finalpostion = this.transform.position;
        manipulator.enabled = false;
        Debug.Log("Anchored at: "+finalpostion);
        
    }

    public void IsReleased() {
        IsAnchored();

    }

}
