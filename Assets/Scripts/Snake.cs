using System.Collections;
using UnityEngine;

public enum SnakeState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    GoToFirstPosition,
}

public class Snake : MonoBehaviour
{
    public Vector3 PatrolPosition;
    public GameObject DeathVFX;

    public float MoveSpeed = 2f;
    public float RotateSpeed = 8f;
    public float JumpForce = 2f;
    public float SnakeDetectionRange = 3f;
    public float AttackRange = 1f;
    public float AttackDamage = 1;
    public float _AttackCooldown = 4f;
    public float IgnoreDistance = 5f;
    public bool IsMover;

    private SnakeState currentState;
    private Vector3 nextPosition;
    private Animator anim;
    private Rigidbody rb;
    private float distanceToPlayer;
    private bool canAttack = true;
    private Transform Player;
    private PlayerStats playerStats;
    private Vector3 PointA;
    private Vector3 PointB;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PointA = transform.position;
        PointB = transform.position + PatrolPosition;
        Player = GameObject.Find("HeadPos").GetComponent<Transform>();
        playerStats = FindFirstObjectByType<PlayerStats>(); // Updated to use FindFirstObjectByType
    }
    void Start()
    {
        nextPosition = PointA;
        if (IsMover)
        {
            currentState = SnakeState.Patrol;
        }
        else
        {
            currentState = SnakeState.Idle;
        }
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
    }
    void ChangeState(SnakeState snakeState)
    {
        currentState = snakeState;
    }
    void RotateTowards(Vector3 _target)
    {
        Vector3 direction = (_target - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }
    IEnumerator AttackCooldown()
    {
        anim.SetTrigger("isAttacking");
        canAttack = false;
        yield return new WaitForSeconds(_AttackCooldown);
        canAttack = true;
    }
    void FixedUpdate()
    {
        switch (currentState)
        {
            case SnakeState.GoToFirstPosition:
                if (Vector3.Distance(transform.position, PointA) < 0.3f)
                {
                    if  (distanceToPlayer >= SnakeDetectionRange)
                    {
                        RotateTowards(Vector3.forward);
                        transform.position = PointA;
                        anim.SetBool("isMoving", false);
                    }
                    else 
                    {
                        ChangeState(SnakeState.Idle);
                    }
                }
                else
                {
                    RotateTowards(PointA);
                    anim.SetBool("isMoving", true);
                    transform.position = Vector3.MoveTowards(transform.position, PointA, MoveSpeed * Time.deltaTime);
                }
                if (distanceToPlayer <= SnakeDetectionRange)
                {
                    ChangeState(SnakeState.Idle);
                }
                break;
            case SnakeState.Idle:
                anim.SetBool("isMoving", false);
                if (distanceToPlayer <= SnakeDetectionRange)
                {
                    //AudioManager.instance.Play("SnakeDetected");
                    RotateTowards(Player.position);
                }
                if (canAttack && distanceToPlayer <= AttackRange)
                {
                    ChangeState(SnakeState.Attack);
                    StartCoroutine(AttackCooldown());
                }
                if (distanceToPlayer >= SnakeDetectionRange)
                {
                    ChangeState(SnakeState.GoToFirstPosition);
                }
                break;
            case SnakeState.Patrol:
                if (IsMover)
                {
                    anim.SetBool("isMoving", true);
                    if (Vector3.Distance(transform.position, nextPosition) < 1f)
                    {
                        nextPosition = nextPosition == PointA ? PointB : PointA;
                    }

                    RotateTowards(nextPosition);
                    transform.position = Vector3.MoveTowards(transform.position, nextPosition, MoveSpeed * Time.deltaTime);
                    if (distanceToPlayer <= SnakeDetectionRange)
                    {
                        AudioManager.instance.Play("SnakeDetected");
                        ChangeState(SnakeState.Chase);
                    }
                }
                break;
            case SnakeState.Chase:
                RotateTowards(Player.position);
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);
                if (distanceToPlayer <= AttackRange && canAttack)
                {
                    ChangeState(SnakeState.Attack);
                    StartCoroutine(AttackCooldown());
                }
                else if (distanceToPlayer >= IgnoreDistance)
                {
                    ChangeState(SnakeState.Patrol);
                }
                break;
            case SnakeState.Attack:
                Vector3 direction = (Player.position - transform.position).normalized;
                rb.AddForce(direction * JumpForce, ForceMode.Impulse);
                if(IsMover)
                {
                    ChangeState(SnakeState.Patrol);
                }
                else
                {
                    ChangeState(SnakeState.Idle);
                }
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Freeze Y rotation
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Unfreeze Y rotation
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, IgnoreDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SnakeDetectionRange);
    }
}
