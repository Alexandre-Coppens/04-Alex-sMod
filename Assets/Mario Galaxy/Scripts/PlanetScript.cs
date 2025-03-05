using CustomGravity;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public float minAttraDist;
    public float maxAttraDist;
    public float attractionForce = 9.8f;
    public float airResistance = 0.98f;
    public PlanetShape planetShape;

    public Planet planet = new();

    private void Start()
    {
        minAttraDist = transform.lossyScale.x * 0.5f;
        if(minAttraDist > transform.lossyScale.y) { minAttraDist = transform.lossyScale.y * 0.5f; }
        if(minAttraDist > transform .lossyScale.z) { minAttraDist = transform .lossyScale.z * 0.5f; }

        switch (planetShape)
        {
            case PlanetShape.Round:
                gameObject.AddComponent<SphereCollider>();
                gameObject.GetComponent<SphereCollider>().isTrigger = true;
                gameObject.GetComponent<SphereCollider>().radius = maxAttraDist / transform.lossyScale.x;
                break;
        }

        planet.Transform = transform;
        planet.AttractionForce = attractionForce;
        planet.AirResistance = airResistance;
        planet.AttractionDistance.minimumDistance = minAttraDist;
        planet.AttractionDistance.maximumDistance = maxAttraDist;
        planet.Shape = planetShape;
    }

    private void Update()
    {
        switch (planetShape)
        {
            case PlanetShape.Round:
                gameObject.GetComponent<SphereCollider>().radius = maxAttraDist / transform.lossyScale.x;
                break;
        }
        Debug.DrawRay(transform.position, transform.up * maxAttraDist, Color.red, 0.01f);
        Debug.DrawRay(transform.position, transform.up * minAttraDist, Color.black, 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectGravity objectGravity = other.GetComponent<ObjectGravity>();
        if (objectGravity == null) return;
        objectGravity.AddPlanet(planet);
    }

    private void OnTriggerExit(Collider other)
    {
        ObjectGravity objectGravity = other.GetComponent<ObjectGravity>();
        if (objectGravity == null) return;
        objectGravity.RemovePlanet();
    }

    private void OnDrawGizmos()
    {
        switch (planetShape)
        {
            case PlanetShape.Round:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, maxAttraDist);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(transform.position, minAttraDist);
                break;

            case PlanetShape.Flat:
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position + Vector3.up * (maxAttraDist * 0.5f), new Vector3(transform.lossyScale.x, maxAttraDist, transform.lossyScale.z));
                Gizmos.color = Color.gray;
                Gizmos.DrawWireCube(transform.position + Vector3.up * (minAttraDist * 0.5f), new Vector3(transform.lossyScale.x, minAttraDist, transform.lossyScale.z));
                break;
        }
        
    }
}
