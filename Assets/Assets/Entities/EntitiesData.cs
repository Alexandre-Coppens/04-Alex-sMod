using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesData", menuName = "DataAssets/Entities", order = 1)]
public class EntitiesData : ScriptableObject
{
    [Header("UI")]
    public string name = "Name";
    public Texture2D image;

    [Header("Variables")]
    public float walkingSpeed = 5f;
    public float health = 100;

    [Header("Vision")]
    public float visionDist = 10;
    [Range(-1, 1)] public float visionDegrees = 0.3f;
    public LayerMask playerLayer;

    [Header("Attack")]
    public float meleeRange = 2;
    public float meleeAttackTime = 0.5f;

    [Header("AI")]
    public EntityIA entityIA = EntityIA.None;

    public enum EntityIA
    {
        None,
        Hostile,
        Friendly,
    }
}
