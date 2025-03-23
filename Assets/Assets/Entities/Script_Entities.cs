using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Script_Entities : MonoBehaviour
{
    [SerializeField] private EntitiesData entitiesData;
    private Player player;

    public float currentHealth;
    public bool isDead = false;
    public EntitiesData.EntityIA entityIA;

    [Header("Vision")]
    [SerializeField] private bool seePlayer;

    [Header("Attack")]
    [SerializeField] private float lastMeleeAttack;
    [SerializeField] private bool inMeleeRange;
    [SerializeField] private bool isAttacking;

    [Header("Movements")]
    [SerializeField] private Vector3 moveToward;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    void Start()
    {
        player = Player.instance;
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        navMeshAgent.speed = entitiesData.walkingSpeed;
        currentHealth = entitiesData.health;
        entityIA = entitiesData.entityIA;
        moveToward = transform.position;
    }
    
    void Update()
    {
        switch (entityIA)
        {
            case EntitiesData.EntityIA.None:
                break;

            case EntitiesData.EntityIA.Hostile:
                HostileIA();
                break;

            case EntitiesData.EntityIA.Friendly:
                break;
        }
        AdditionalScript();
    }

    private void HostileIA()
    {
        if (seePlayer)
        {
            MeleeRange();
            Attack();
            Movement();
        }
        else SeeAround();
    }

    private void SeeAround()
    {
        if(Physics.Raycast(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized, entitiesData.visionDist, entitiesData.playerLayer))
        {
            if (Vector3.Dot(transform.forward, ((player.transform.position - transform.position)).normalized) > entitiesData.visionDegrees) 
            { 
                seePlayer = true; 
                return;
            }
        }
        seePlayer = false;
    }

    private void MeleeRange()
    {
        inMeleeRange = Physics.Raycast(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized, entitiesData.meleeRange, entitiesData.playerLayer);
        Debug.DrawRay(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized * entitiesData.meleeRange, Color.blue);
    }

    private void Attack()
    {
        if (inMeleeRange && Time.time >= lastMeleeAttack + entitiesData.meleeAttackTime)
        {
            animator.SetTrigger("Attack_Melee");
            lastMeleeAttack = Time.time;
            IEnumerator Coroutine = WaitForEndAttack(entitiesData.meleeAttackTime);
            StartCoroutine(Coroutine);
        }
    }

    private void Movement()
    {
        if(Vector3.Distance(transform.position, player.transform.position) > entitiesData.meleeRange)
        {
            moveToward = player.transform.position;
            navMeshAgent?.SetDestination(moveToward);
        }
        animator.SetFloat("Moving", rb.velocity.magnitude);
    }

    public void TakeDamage(float damage)
    {
        if(isDead) return;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("Die");
            entityIA = EntitiesData.EntityIA.None;
        }
    }

    public virtual void AdditionalScript()
    {
        return;
    }

    private IEnumerator WaitForEndAttack(float time)
    {
        yield return new WaitForSeconds(time);
        moveToward = player.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        if(seePlayer)Gizmos.color = Color.yellow;
        else Gizmos.color = Color.red;
        if ((player != null)) Gizmos.DrawRay(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f)).normalized) * entitiesData.visionDist);
        else Gizmos.DrawRay(transform.position, transform.forward * entitiesData.visionDist);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + GetComponent<CapsuleCollider>().center, entitiesData.meleeRange);

    }
}
