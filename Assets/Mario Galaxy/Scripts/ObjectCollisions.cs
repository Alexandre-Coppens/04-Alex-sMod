using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisions : MonoBehaviour
{
    [Tooltip("X = Right, Y = Up, Z = Forward")]
    [SerializeField] private Vector3 collision;

    [SerializeField] private LayerMask floorMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, collision.z, floorMask)) transform.position -= transform.forward * (collision.z - hit.distance);
        if(Physics.Raycast(transform.position, -transform.forward, out hit, collision.z, floorMask)) transform.position -= -transform.forward * (collision.z - hit.distance);
        if(Physics.Raycast(transform.position, transform.right, out hit, collision.z, floorMask)) transform.position -= transform.right * (collision.x - hit.distance);
        if(Physics.Raycast(transform.position, -transform.right, out hit, collision.z, floorMask)) transform.position -= -transform.right * (collision.x - hit.distance);
        if(Physics.Raycast(transform.position, transform.up, out hit, collision.z, floorMask)) transform.position -= transform.up * (collision.y - hit.distance);
        if(Physics.Raycast(transform.position, -transform.up, out hit, collision.z, floorMask)) transform.position -= -transform.up * (collision.y - hit.distance);

        if(Physics.Raycast(transform.position, (transform.forward + transform.right).normalized, out hit, (collision.x + collision.z) * 0.50f, floorMask)) 
            transform.position -= (transform.forward + transform.right).normalized * (((collision.x + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (transform.forward + -transform.right).normalized, out hit, (collision.x + collision.z) * 0.50f, floorMask))
            transform.position -= (transform.forward + -transform.right).normalized * (((collision.x + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.forward + transform.right).normalized, out hit, (collision.x + collision.z) * 0.50f, floorMask))
            transform.position -= (-transform.forward + transform.right).normalized * (((collision.x + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.forward + -transform.right).normalized, out hit, (collision.x + collision.z) * 0.50f, floorMask))
            transform.position -= (-transform.forward + -transform.right).normalized * (((collision.x + collision.z) * 0.50f) - hit.distance);

        if (Physics.Raycast(transform.position, (transform.forward + transform.up).normalized, out hit, (collision.x + collision.y) * 0.50f, floorMask))
            transform.position -= (transform.forward + transform.up).normalized * (((collision.x + collision.y) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (transform.forward + -transform.up).normalized, out hit, (collision.x + collision.y) * 0.50f, floorMask))
            transform.position -= (transform.forward + -transform.up).normalized * (((collision.x + collision.y) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.forward + transform.up).normalized, out hit, (collision.x + collision.y) * 0.50f, floorMask))
            transform.position -= (-transform.forward + transform.up).normalized * (((collision.x + collision.y) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.forward + -transform.up).normalized, out hit, (collision.x + collision.y) * 0.50f, floorMask))
            transform.position -= (-transform.forward + -transform.up).normalized * (((collision.x + collision.y) * 0.50f) - hit.distance);

        if (Physics.Raycast(transform.position, (transform.up + transform.right).normalized, out hit, (collision.y + collision.z) * 0.50f, floorMask))
            transform.position -= (transform.up + transform.right).normalized * (((collision.y + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (transform.up + -transform.right).normalized, out hit, (collision.y + collision.z) * 0.50f, floorMask))
            transform.position -= (transform.up + -transform.right).normalized * (((collision.y + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.up + transform.right).normalized, out hit, (collision.y + collision.z) * 0.50f, floorMask))
            transform.position -= (-transform.up + transform.right).normalized * (((collision.y + collision.z) * 0.50f) - hit.distance);
        if (Physics.Raycast(transform.position, (-transform.up + -transform.right).normalized, out hit, (collision.y + collision.z) * 0.50f, floorMask))
            transform.position -= (-transform.up + -transform.right).normalized * (((collision.y + collision.z) * 0.50f) - hit.distance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * collision.z);
        Gizmos.DrawLine(transform.position, transform.position + -transform.forward * collision.z);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * collision.x);
        Gizmos.DrawLine(transform.position, transform.position + -transform.right * collision.x);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * collision.y);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * collision.y);

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + transform.right).normalized * (collision.x + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + -transform.right).normalized * (collision.x + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.forward + transform.right).normalized * (collision.x + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.forward + -transform.right).normalized * (collision.x + collision.z) * 0.50f);

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + transform.up).normalized * (collision.x + collision.y) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + -transform.up).normalized * (collision.x + collision.y) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.forward + transform.up).normalized * (collision.x + collision.y) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.forward + -transform.up).normalized * (collision.x + collision.y) * 0.50f);

        Gizmos.DrawLine(transform.position, transform.position + (transform.up + transform.right).normalized * (collision.y + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.up + -transform.right).normalized * (collision.y + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up + transform.right).normalized * (collision.y + collision.z) * 0.50f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up + -transform.right).normalized * (collision.y + collision.z) * 0.50f);
    }
}
