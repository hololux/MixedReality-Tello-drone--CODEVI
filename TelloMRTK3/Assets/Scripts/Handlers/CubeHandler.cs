using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    public GameObject cube;

    private void Update()
    {
        Vector3 objectPosition= cube.transform.position;
        Rigidbody objectphysics =GetComponent<Rigidbody>();

        
        Debug.Log(objectPosition);
        Debug.Log(objectphysics.velocity);

    }

}
