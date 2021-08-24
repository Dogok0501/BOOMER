using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    public Transform[] waypoints;
    public int patrolRange = 40;     // 정찰 시 시야 거리
    public int shootRange;      // 원거리 공격 가능 범위
    public int attackRange;     // 근접 공격 가능 범위
    public Transform vision;
    public float stayAlertTime;
    public float viewAngle;

    public BoxCollider boxCollider;
    public new Rigidbody rigidbody;
    public GameObject effect;
    public GameObject playerSpotted;

    public float angrySpeed;
    public float normalSpeed;

    //원거리 공격
    public GameObject missile;
    public GameObject duckGrenade;
    public float missileDamage;
    public float missileSpeed;  // 투사체 속도

    //근거리 공격
    public bool onlyMelee = false;
    public GameObject shockwave;
    public float meleeDamage;
    public float attackDelay;   // 공격 쿨타임 (=공격속도)

    public LayerMask raycastMask;
    public Animator anim;

    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public AttackState attackState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public BossAttackState bossAttackState;
    [HideInInspector]
    public IEnemyAI currentState;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public Vector3 lastKnownPosition;

    private void Awake()
    {
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        bossAttackState = new BossAttackState(this);
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (!CompareTag("Boss"))
        {
            currentState = patrolState;
            playerSpotted.gameObject.SetActive(false);
        }     
        else
        {
            currentState = bossAttackState;
            effect = transform.Find("Effect").gameObject;
        }            
    }

    void Update()
    {
        currentState.UpdateAction();
        Debug.Log(EnemySpotted());
    }

    private void OnTriggerEnter(Collider otherObj)
    {
        currentState.OnTriggerEnter(otherObj);
    }

    private void HiddenShot(Vector3 shotPosition)
    {
        if (!CompareTag("Boss"))
        {
            Debug.Log("Hidden shot");
            lastKnownPosition = shotPosition;
            currentState = alertState;
        }            
    }

    public bool EnemySpotted()
    {
        Vector3 direction = GameObject.FindWithTag("Player").transform.position - transform.position;
        float angle = Vector3.Angle(direction, vision.forward);

        if (angle < viewAngle * 0.5f)
        {       
            RaycastHit hit;
            if (Physics.Raycast(vision.transform.position, direction.normalized, out hit, patrolRange, raycastMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    chaseTarget = hit.transform;
                    lastKnownPosition = hit.transform.position;
                    transform.LookAt(hit.transform);
                    return true;
                }
            }
        }
        return false;
    }

    Vector3 DirFromAngle(float angleInDegree)
    {
        return new Vector3(Mathf.Sin(angleInDegree * 0.5f * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * 0.5f * Mathf.Deg2Rad));
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, (vision.forward + DirFromAngle(viewAngle)) * patrolRange, Color.red);
        Debug.DrawRay(transform.position, (vision.forward + DirFromAngle(-viewAngle)) * patrolRange, Color.blue);
        Debug.DrawRay(transform.position, vision.forward * patrolRange, Color.green);
    }
}
