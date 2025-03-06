using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatearound : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 rotateAround;
    [SerializeField] private Vector3 rotationVector;

    [SerializeField] private Vector2 maxTopDown;

    // Update is called once per frame
    void Update()
    {
        float D = Mathf.Sqrt(Mathf.Pow(distance*-10,2) - Mathf.Pow(distance - (rotateAround.y - transform.position.y) , 2));
        float X = rotateAround.x + Mathf.Cos(speed * Time.time) * D;
        float Z = rotateAround.z + Mathf.Sin(speed * Time.time) * D;
        transform.position = new Vector3(X, Mathf.Clamp(transform.position.y, rotateAround.y + maxTopDown.y, rotateAround.y + maxTopDown.x), Z);
        transform.LookAt(rotateAround, transform.up);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rotateAround, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rotateAround + new Vector3(0, maxTopDown.x), rotateAround + new Vector3(0, maxTopDown.y));
        Gizmos.DrawLine(transform.position, new Vector3(rotateAround.x, transform.position.y, rotateAround.z));
    }
}
