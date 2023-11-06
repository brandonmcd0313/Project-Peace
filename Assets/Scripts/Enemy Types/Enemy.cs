using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //variables that change with type of enemy
    protected int attackDamage;
    protected float attackCooldown;
    protected float speed;


    //all enemies use the same 
    protected bool canAttack;
    protected bool isAttacking;
    protected EnemyController controllerInstance;
    protected GameObject player;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        canAttack = false;
        player = GameObject.FindGameObjectWithTag("Player");
        controllerInstance = FindObjectOfType<EnemyController>();
        controllerInstance.OnPlayerAttackEnemy += OnAttacked;
        Physics.IgnoreLayerCollision(3, 3); //ignore collsions with other objects in the enemy layer
        Physics.IgnoreLayerCollision(3, 3); //ignore collsions with other objects in the enemy layer
    }

    protected virtual void Attack(GameObject obj)
    {
        if(!canAttack)
        {
            return;
        }
        
        if (obj.TryGetComponent(out IDamageable damageableObject))
        {
            damageableObject.Damage(attackDamage);
            print(this.gameObject.name + " attacked " + obj.name + " for " + attackDamage + " damage");
        }
    }

    protected virtual void OnAttacked()
    {
        canAttack = true;
        isAttacking = true;
    }
    
    protected virtual void TryAttack(GameObject obj)
    {
        if (!isAttacking)
        {
            //first time player is attacked
            controllerInstance.OnPlayerAttackEnemy?.Invoke();
            
        }
        else
        {
            Attack(obj);
        }
    }
    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
