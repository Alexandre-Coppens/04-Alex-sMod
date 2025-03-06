using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunScript : MonoBehaviour
{
    [Header("Variables")]
    public WeaponsData weaponData;

    [Header("VFXs & others")]
    public VisualEffect muzzleVFX;
    public GameObject PREFAB_bullet;
    public GameObject PREFAB_Decal;
    [SerializeField] private float lastFiredTime = 0f;
    [SerializeField] private bool debug = false;

    Animator animator;
    [HideInInspector] Vector3 rayHit;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        if (Time.realtimeSinceStartup >= lastFiredTime + weaponData.fireRate)
        {
            animator.SetTrigger("Shoot");
            lastFiredTime = Time.realtimeSinceStartup;

            Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
            Debug.DrawRay(ray.origin, ray.direction * weaponData.maxDistance, Color.red, 3f);
            RaycastHit hit;
            rayHit = Vector3.zero;
            if (Physics.Raycast(ray, out hit, weaponData.maxDistance))
            {
                rayHit = hit.point;
                GameObject decal = Instantiate(PREFAB_Decal,
                                               rayHit, 
                                               transform.rotation * new Quaternion(0, 180, 0, 0));
            }
            else
            {
                rayHit = gameObject.transform.forward + new Vector3(0, weaponData.maxDistance);
            }

            GameObject bullet = Instantiate(PREFAB_bullet, 
                                            transform.position + new Vector3(0, 0.3f), 
                                            transform.rotation * new Quaternion(0, 180, 0, 0));
            Bullet_Behaviour bb = bullet.GetComponent<Bullet_Behaviour>();
            bb.Bullet_Start(rayHit);
        }
    }

    // Update is called once per frame
    public void GunUpdate(bool isRunning, bool isShooting)
    {
        animator.SetBool("isRunning", isRunning);
        if (isShooting) 
        { 
            Shoot(); 
        };
    }
}
