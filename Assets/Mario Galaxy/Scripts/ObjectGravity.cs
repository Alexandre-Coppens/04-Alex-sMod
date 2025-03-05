using CustomGravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{

    [Header("Object variable")]
    public float objectMass = 1.0f;
    public Vector3 addedMovement;
    private Rigidbody rb;

    [Header("Planets Variables")]
    [SerializeField] private Planet planet;

    [Header("Planets Calculations")]
    public float gravity;

    [Header("Gravity")]
    public bool grounded;
    public float groundDistance;
    public Vector3 velocity;
    public Vector3 movementVelocity;
    public Vector3 downAxis;
    public Vector3 lastPoint;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask objectLayer;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
    }

    void Update()
    {
        movementVelocity = addedMovement;
        rb.velocity = velocity + movementVelocity;
        velocity += downAxis * gravity * objectMass * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        PlanetCalculations();
    }

    void PlanetCalculations()
    {
        switch (planet.Shape)
        {
            case PlanetShape.Void:
                downAxis = new Vector3(0, 0, 0);
                return;

            case PlanetShape.Flat:
                downAxis = new Vector3(0, -1, 0);
                break;

            case PlanetShape.Round:
                downAxis = (planet.Transform.position - transform.position).normalized;
                break;
        }
        Debug.DrawRay(transform.position, downAxis, Color.blue, 0.01f);

        gravity = planet.AttractionForce;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, downAxis, out hit, _rayDist, objectLayer)) 
        { 
            if(hit.distance > groundDistance) return;
            velocity = Vector3.zero; 
            transform.position = hit.point + groundDistance * -downAxis;
            grounded = true;
            lastPoint = hit.point;
        }
    }

    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    public void AddPlanet(Planet _newPlanet)
    {
        planet = _newPlanet;
    }

    public void RemovePlanet()
    {
        planet.Shape = PlanetShape.Void;
    }

    public void AddImpulse(Vector3 impulse)
    {
        Vector3 deplacement = transform.forward * velocity.z + -downAxis * velocity.y + transform.right * velocity.x;
        velocity += impulse;
        transform.localPosition += deplacement * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lastPoint, 0.5f);
        Gizmos.DrawRay(transform.position, downAxis * 2);
    }
}