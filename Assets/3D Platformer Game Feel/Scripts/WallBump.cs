using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBump : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Dot(transform.forward, other.transform.forward) > 0.5 && Vector3.Dot(transform.position - other.transform.position, other.transform.forward) > 0.5)
        {
            Debug.Log("Hit");   
        }
    }
}
