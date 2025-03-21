using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Script_Weapons : MonoBehaviour
{
    public bool onTheFloor = false;

    [Header("Variables")]
    public WeaponsData weaponData;
    public LayerMask enemyLayer;

    [Header("VFXs & others")]
    public VisualEffect muzzleVFX;
    public GameObject PREFAB_bullet;
    public GameObject PREFAB_Decal;
    [SerializeField] private float lastFiredTime = 0f;
    [SerializeField] private bool debug = false;

    public Animator animator;
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

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * weaponData.maxDistance, Color.red, 3f);
            RaycastHit hit;
            rayHit = Vector3.zero;
            if (Physics.Raycast(ray, out hit, weaponData.maxDistance))
            {
                Debug.Log(hit.collider.name);
                HitActor(hit);
                rayHit = hit.point;
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

    public virtual void HitActor(RaycastHit hit)
    {
        //GameObject decal = Instantiate(PREFAB_Decal,
        //                                       hit.point,
        //                                       transform.rotation * new Quaternion(0, 180, 0, 0));

        Script_Entities hitActor = hit.collider.gameObject.GetComponent<Script_Entities>();
        if ( hitActor != null)
        {
            hitActor.TakeDamage(weaponData.damages);
        }
    }

    public virtual void AdditionalAction()
    {
        return;
    }

    // Update is called once per frame
    public void GunUpdate(bool isRunning, bool isShooting)
    {
        AdditionalAction();
        animator.SetBool("isRunning", isRunning);
        if (isShooting)
        {
            Shoot();
        };
    }
}