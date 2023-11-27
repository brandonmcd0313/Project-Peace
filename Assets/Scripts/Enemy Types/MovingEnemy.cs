using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingEnemy : Enemy
{
    [Header("Enemy Variables")]
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackCooldown;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _health;
    [SerializeField] float _damageForce;
    [SerializeField] bool _useDamageForce;
    [SerializeField] bool _canMoveOnYAxis;

    [Header("Enemy Animations")]
    [SerializeField] AnimationClip _idleAnimation;
    [SerializeField] AnimationClip _attackAnimation;
    [SerializeField] AnimationClip _deathAnimation;
    [SerializeField] AnimationClip _moveAnimation;

    [Header("Pathfinding Values")]
    [SerializeField] float _speed;
    [SerializeField] GameObject[] _pathfindingPoints;

    private Animator anim;
    private Vector3 defaultScale;
    private enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Die
    }

    private EnemyState currentState = EnemyState.Idle;

    // Start is called before the first frame update
    protected override void Start()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        anim = GetComponent<Animator>();
        base.Start();

        Health = _health;
        attackDamage = _attackDamage;
        attackCooldown = _attackCooldown;
        speed = _attackSpeed;
        damageForce = _damageForce;
        useDamageForce = _useDamageForce;
        canMoveOnYAxis = _canMoveOnYAxis;

        anim.SetBool("move", true);
       anim.SetBool("idle", false);
     
        defaultScale = transform.localScale;

        StartCoroutine(Movement());
    }

    protected override void OnAttacked()
    {
        base.OnAttacked();
        StopAllCoroutines();
        // Start simulating the rigidbody on this object so it can fall
        GetComponent<Rigidbody2D>().isKinematic = false;
        // Change the state to Attack
        currentState = EnemyState.Attack;
    }

    protected override void Attack(GameObject obj)
    {

        if (!canAttack)
        {
            return;
        }

        // Change the state to Attack
        currentState = EnemyState.Attack;

        anim.Play(_attackAnimation.name, -1, 0);
        StartCoroutine(AttackMoment(obj, 0.5f));
    }
    IEnumerator AttackMoment(GameObject target, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        base.Attack(target);

        // After attacking, return to the Move state
        currentState = EnemyState.Move;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == EnemyState.Move)
        {
            
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        if (!_canMoveOnYAxis)
        {
            moveDirection.y = 0;
        }

        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            OnAttacked();
            Attack(collision.gameObject);
        }
    }

    IEnumerator Movement()
    {
        while (true)
        {
            for (int i = 0; i < _pathfindingPoints.Length; i++)
            {
                Vector3 targetPosition = _pathfindingPoints[i].transform.position;
                if (!_canMoveOnYAxis)
                {
                    targetPosition.y = transform.position.y;
                }

                while (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    MoveTowardsPoint(targetPosition);
                    yield return new WaitForEndOfFrame();
                }
            }

            for (int i = _pathfindingPoints.Length - 1; i >= 0; i--)
            {
                Vector3 targetPosition = _pathfindingPoints[i].transform.position;
                if (!_canMoveOnYAxis)
                {
                    targetPosition.y = transform.position.y;
                }

                while (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    MoveTowardsPoint(targetPosition);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
    
    private void MoveTowardsPoint(Vector3 targetPosition)
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    protected override void OnDeathInstance()
    {
        // Change the state to Die
        currentState = EnemyState.Die;

        anim.Play(_deathAnimation.name, -1, 0);

        // Wait for the animation to finish
        Invoke("Die", 0.5f);
    }

    void Die()
    {
        base.OnDeath();
    }
}
