using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    //variables that change with type of enemy
    protected int attackDamage;
    protected float attackCooldown;
    protected float speed;


    //all enemies use the same 
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
            damageableObject.Damage(attackDamage);
            print(this.gameObject.name + " attacked " + obj.name + " for " + attackDamage + " damage");
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
            OnDeath();
        }
       
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
