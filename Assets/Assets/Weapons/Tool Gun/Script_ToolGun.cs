using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ToolGun : Script_Weapons
{
    Player_Inputs Player_Inputs;
    public CurrentFunction currentFunction = CurrentFunction.SpawnActor;
    public bool hasChanged = false;

    public GameObject[] entitiesSpawnList;
    public GameObject[] objectSpawnList;

    public enum CurrentFunction
    {
        SpawnActor,
        SpawnProps,
        Erase,
    }

    private void Start()
    {
        Player_Inputs = Player_Inputs.instance;
    }

    public override void AdditionalAction()
    {
        if (Player_Inputs.actionChanged)
        {
            if (!hasChanged)
            {
                hasChanged = true;
                ChangeAction();
            }
        }
        else
        {
            hasChanged = false;
        }
    }

    public void ChangeAction()
    {
        switch (currentFunction)
        {
            case CurrentFunction.SpawnActor:
                currentFunction = CurrentFunction.SpawnProps; 
                break;

            case CurrentFunction.SpawnProps:
                currentFunction = CurrentFunction.Erase;
                break;

            case CurrentFunction.Erase:
                currentFunction = CurrentFunction.SpawnActor;
                break;
        }
    }

    public override void HitActor(RaycastHit hit)
    {
        switch (currentFunction)
        {
            case CurrentFunction.SpawnActor:
                Instantiate(entitiesSpawnList[Random.Range(0, entitiesSpawnList.Length)], hit.point, Quaternion.identity);
                break;

            case CurrentFunction.SpawnProps:
                GameObject item = Instantiate(objectSpawnList[Random.Range(0, objectSpawnList.Length)], hit.point, Quaternion.identity);
                item.tag = "Actor";
                item.layer = 10;
                item.AddComponent<MeshCollider>();
                break;

            case CurrentFunction.Erase:
                if(hit.collider.tag == "Actor")
                {
                    Destroy(hit.collider.gameObject);
                }
                break;
        }
    }

}
