using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    //variables that change with type of enemy
    protected int attackDamage;
    protected float attackCooldown;
    protected float speed;
    protected float damageForce;
    protected bool useDamageForce;
    protected bool canMoveOnYAxis;


    //all enemies use the same 
    protected bool isPreformingAttack = false;
    protected bool canAttack = false;
    protected bool isAttacking = false;
    protected EnemyController controllerInstance;
    protected GameObject player;

    public int Health { get; set; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isAttacking = false;
        player = GameObject.FindGameObjectWithTag("Player");
        controllerInstance = FindObjectOfType<EnemyController>();
        controllerInstance.OnPlayerAttackEnemy += OnAttacked;
      
    }

    protected virtual void Attack(GameObject obj)
    {
       
        if(!canAttack)
        {
            return;
        }
        
        if (obj.TryGetComponent(out IDamageable damageableObject))
        {
            isPreformingAttack = true;
            damageableObject.Damage(attackDamage);
            print(this.gameObject.name + " attacked " + obj.name + " for " + attackDamage + " damage");

            if(!useDamageForce)
            {
                return;
            }

            //apply a force in the opposite direction of the obj
            Vector2 forceDirection = (transform.position - obj.transform.position).normalized;
            if (!canMoveOnYAxis)
            {
                forceDirection.y = 0;
            }
            print("applied force");
            GetComponent<Rigidbody2D>().AddForce(damageForce * forceDirection, ForceMode2D.Impulse);

            //apply a force on the obj being attacked
            obj.GetComponent<Rigidbody2D>().AddForce(-damageForce/2 * forceDirection, ForceMode2D.Impulse);


        }
    }

    protected virtual void OnAttacked()
    {
        if (isAttacking)
        {
            return;
        }
            print(gameObject.name + " was attacked");
            canAttack = true;
            isAttacking = true;
        //activate for other enemies
        controllerInstance.OnPlayerAttackEnemy?.Invoke();
        
        
    }
    
    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            OnDeathInstance();
        }
       
    }

    public void OnDeath()
    {
        controllerInstance.OnPlayerAttackEnemy -= OnAttacked;
        Destroy(gameObject);
    }

    protected virtual void OnDeathInstance()
    {
        //the instnace allows each enemy to implement death differently
        OnDeath();
    }
}
