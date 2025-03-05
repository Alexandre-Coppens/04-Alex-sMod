using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunScript : MonoBehaviour
{
    [Header("Variables")]
    public float timeShoot = 0f;

    [Header("VFXs & others")]
    public VisualEffect muzzleVFX;
    public GameObject parent;
    public GameObject PREFAB_bullet;
    public GameObject PREFAB_Decal;
    [SerializeField]float time = 0f;
    [SerializeField] bool debug= false;

    Animator animator;
    [HideInInspector] Vector3 rayHit;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void GunUpdate(bool isRunning, bool isShooting, float delta)
    {
        debug = delta - time >= timeShoot;
        if(delta - time >= timeShoot)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isShooting", false);
            if (isShooting && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Pistol_Shoot")
            {
                animator.SetBool("isShooting", true);
                time = delta;
                //muzzleVFX.Play();

                Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
                float rayLength = 500f;
                Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 3f);
                RaycastHit hit;
                rayHit = Vector3.zero;
                if (Physics.Raycast(ray, out hit, rayLength))
                {
                    rayHit = hit.point;
                    GameObject decal = Instantiate(PREFAB_Decal);
                    decal.transform.position = rayHit;
                    decal.transform.rotation = parent.transform.rotation * new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    rayHit = gameObject.transform.forward + new Vector3(0, rayLength);
                }

                GameObject bullet = Instantiate(PREFAB_bullet);
                Bullet_Behaviour bb = bullet.GetComponent<Bullet_Behaviour>();
                bullet.transform.position = parent.transform.position + new Vector3(0, 0.3f);
                bullet.transform.rotation = parent.transform.rotation * new Quaternion(0, 180, 0, 0);
                bb.Bullet_Start(rayHit);
            }
        }
    }
}
