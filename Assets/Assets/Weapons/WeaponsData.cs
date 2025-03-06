using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsData", menuName = "DataAssets/Weapons", order = 1)]
public class WeaponsData : ScriptableObject
{
    public string name = "Name";
    public float damages = 30;
    public float fireRate = 0.3f;
    public float maxDistance = 500f;
}
