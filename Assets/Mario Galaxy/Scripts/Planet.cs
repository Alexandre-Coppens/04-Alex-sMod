using UnityEngine;

namespace CustomGravity
{
    public struct Planet
    {
        public Transform Transform;
        public PlanetShape Shape;
        public float AttractionForce;
        public AttractionField AttractionDistance;
        public float AirResistance;
    }

    public enum PlanetShape
    {
        Void,
        Round,
        Flat,
    }

    public struct AttractionField
    {
        public float minimumDistance;
        public float maximumDistance;
    }
}
