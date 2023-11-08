using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [Header("Pathfinding Values")]
    [SerializeField] float _speed;
    [SerializeField] GameObject[] _pathfindingPoints;
    // Start is called before the first frame update
    protected override void Start()
    {

        GetComponent<Rigidbody2D>().isKinematic = true;
        StartCoroutine(Movement());


        Health = _health;
        attackDamage = _attackDamage;
        attackCooldown = _attackCooldown;
        speed = _attackSpeed;
        damageForce = _damageForce;
        useDamageForce = _useDamageForce;
        canMoveOnYAxis = _canMoveOnYAxis;

        base.Start();

    }

    protected override void OnAttacked()
    {
        base.OnAttacked();
        StopAllCoroutines();
        GetComponent<Rigidbody2D>().isKinematic = false;

    }

    protected override void Attack(GameObject obj)
    {
        base.Attack(obj);
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAttacking)
        {
            return;
        }

        //move towards the player on x axis only
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        if (!_canMoveOnYAxis)
        {
            moveDirection.y = 0;
        }
        // Move the object towards the destination
        transform.Translate(moveDirection * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            //it has both been attacked and is trying to attack
            OnAttacked();
            Attack(collision.gameObject);
        }

    }

    IEnumerator Movement()
    {
        while (true)
        {
            //move to each point 
            for (int i = 0; i < _pathfindingPoints.Length; i++)
            {
                Vector3 targetPosition = _pathfindingPoints[i].transform.position;
                if (!_canMoveOnYAxis)
                {
                    targetPosition.y = transform.position.y;
                }
                while (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    //move towards the player on x axis only
                    Vector3 moveDirection = (targetPosition - transform.position).normalized;

                    // Move the object towards the destination
                    transform.Translate(moveDirection * speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }

            }

            //move back in opposite direction
            for (int i = _pathfindingPoints.Length - 1; i >= 0; i--)
            {
                Vector3 targetPosition = _pathfindingPoints[i].transform.position;
                if (!_canMoveOnYAxis)
                {
                    targetPosition.y = transform.position.y;
                }
                while (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    //move towards the player on x axis only
                    Vector3 moveDirection = (targetPosition - transform.position).normalized;

                    // Move the object towards the destination
                    transform.Translate(moveDirection * speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }

            }
        }
    }
}
