using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet_Behaviour : MonoBehaviour
{
    public void Bullet_Start(Vector3 hitPoint)
    {
        VisualEffect visualEffect = GetComponent<VisualEffect>();
        //visualEffect.SetVector3(0, transform.position);
        //visualEffect.SetVector3(1, hitPoint);
        visualEffect.Play();
        float time = Time.realtimeSinceStartup;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
