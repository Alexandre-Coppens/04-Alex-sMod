using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesData", menuName = "DataAssets/Entities", order = 1)]
public class EntitiesData : ScriptableObject
{
    public string name = "Name";
    public Texture2D image;
    public float speed = 5f;
    public EntityIA entityIA = EntityIA.None;

    public enum EntityIA
    {
        None,
        Hostile,
        Friendly,
    }
}
