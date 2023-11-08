using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
public class StillEnemy : Enemy
{
    [Header("Enemy Variables")]
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackCooldown;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _health;
    [SerializeField] float _damageForce;
    [SerializeField] bool _useDamageForce;
    [SerializeField] bool _canMoveOnYAxis;

    // Start is called before the first frame update
    protected override void Start()
    {
        //prevent the object from moving due to gravity
        GetComponent<Rigidbody2D>().isKinematic = true;
        base.Start();

        Health = _health;
        attackDamage = _attackDamage;
        attackCooldown = _attackCooldown;
        speed = _attackSpeed;
        damageForce = _damageForce;
        useDamageForce = _useDamageForce;
        canMoveOnYAxis = _canMoveOnYAxis;
    }

    protected override void OnAttacked()
    {
        base.OnAttacked();
        //start simulating the rigidbody on this object so it can fall
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAttacking)
        {
            return;
        }

        //move towards the player on x axis only
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        moveDirection.y = 0;
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
}
