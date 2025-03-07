using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Script_Entities : MonoBehaviour
{
    [SerializeField] private EntitiesData entitiesData;
    private Player player;

    //REMEMBER TO MIGRATE THE VARIABLES IN THE DATA_ASSET

    [Header("Vision")]
    [SerializeField] private bool seePlayer;//
    [SerializeField] private float visionDist;
    [SerializeField, Range(-1, 1)] private float visionDegrees;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack")]
    [SerializeField] private float meleeRange;
    [SerializeField] private float meleeAttackTime;
    [SerializeField] private float lastMeleeAttack;//
    [SerializeField] private bool inMeleeRange;//
    [SerializeField] private bool isAttacking;//

    [Header("Movements")]
    [SerializeField] private Vector3 moveToward;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;//

    void Start()
    {
        player = Player.instance;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        moveToward = transform.position;
    }
    
    void Update()
    {
        switch (entitiesData.entityIA)
        {
            case EntitiesData.EntityIA.None:
                break;

            case EntitiesData.EntityIA.Hostile:
                HostileIA();
                break;

            case EntitiesData.EntityIA.Friendly:
                break;
        }
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
        if(Physics.Raycast(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized, visionDist, playerLayer))
        {
            if (Vector3.Dot(transform.forward, ((player.transform.position - transform.position)).normalized) > visionDegrees) 
            { 
                seePlayer = true; 
                return;
            }

        }
        seePlayer = false;
    }

    private void MeleeRange()
    {
        inMeleeRange = Physics.Raycast(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized, meleeRange, playerLayer);
        Debug.DrawRay(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f))).normalized * meleeRange, Color.blue);
    }

    private void Attack()
    {
        if (inMeleeRange && Time.time >= lastMeleeAttack + meleeAttackTime)
        {
            animator.SetTrigger("Attack_Melee");
            lastMeleeAttack = Time.time;
        }
    }

    private void Movement()
    {
        if(Vector3.Distance(transform.position, moveToward) > meleeRange)
        {
            moveToward = player.transform.position;
            navMeshAgent.SetDestination(moveToward);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(seePlayer)Gizmos.color = Color.yellow;
        else Gizmos.color = Color.red;
        if ((player != null)) Gizmos.DrawRay(transform.position, ((player.transform.position - transform.position + new Vector3(0, 0.5f)).normalized) * visionDist);
        else Gizmos.DrawRay(transform.position, transform.forward * visionDist);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

    }
}
