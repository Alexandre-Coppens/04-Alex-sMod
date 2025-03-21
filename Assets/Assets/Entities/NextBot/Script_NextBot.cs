using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBot : Script_Entities
{
    // Update is called once per frame
    public override void AdditionalScript()
    {
        transform.GetChild(0).LookAt(Camera.main.transform.position);
    }
}
