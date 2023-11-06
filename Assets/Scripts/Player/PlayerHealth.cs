using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    //health resets when a new part is entered
    [SerializeField] int _health;
    public int Health { get; set; }

    void Start()
    {
        Health = _health;
    }
    public void Damage(int damage)
    {
        Health -= damage;
        if(Health == 0)
        {
            OnDeath();
        
       }
    }

    public void OnDeath()
    {
        //reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    }
