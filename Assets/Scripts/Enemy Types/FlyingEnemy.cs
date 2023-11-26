using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemy : Enemy
{
    [Header("Enemy Variables")]
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackCooldown;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _health;
    [SerializeField] float _damageForce;
    [SerializeField] bool _useDamageForce;
 
    bool _canMoveOnYAxis = true;

    [Header("Enemy Animations")]
    [SerializeField] AnimationClip _idleAnimation;
    [SerializeField] AnimationClip _attackAnimation;
    [SerializeField] AnimationClip _deathAnimation;
    [SerializeField] AnimationClip _moveAnimation;

    private Animator anim;
    private Vector3 defaultScale; private enum EnemyState
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
        // Prevent the object from moving due to gravity
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

        // Play the idle animation as the default
        anim.SetBool("idle", true);

        anim.SetBool("move", false);
        defaultScale = transform.localScale;
    }

    protected override void OnAttacked()
    {
        base.OnAttacked();
        //start simulating the rigidbody on this object
        GetComponent<Rigidbody2D>().isKinematic = false;
        //set gravity to zero
        GetComponent<Rigidbody2D>().gravityScale = 0;
        // Change the state to Attack
        currentState = EnemyState.Attack;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == EnemyState.Move)
        {
            anim.SetBool("move", true);
            anim.SetBool("idle", false);
            //move towards the player 
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;
            transform.Translate(moveDirection * speed * Time.deltaTime);

            // If move direction is to the right, flip the sprite to face the player
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            // It has both been attacked and is trying to attack
            OnAttacked();
            Attack(collision.gameObject);
        }
    }

    protected override void Attack(GameObject target)
    {
        if (!canAttack)
        {
            return;
        }

        // Change the state to Attack
        currentState = EnemyState.Attack;

        anim.Play(_attackAnimation.name, -1, 0);
        StartCoroutine(AttackMoment(target, 0.5f));
    }

    IEnumerator AttackMoment(GameObject target, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        base.Attack(target);

        // After attacking, return to the Move state
        currentState = EnemyState.Move;

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

