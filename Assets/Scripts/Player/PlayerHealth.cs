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
        StartCoroutine(FlashRed());
        if(Health <= 0)
        {
            OnDeath();
        
       }
    }

    public void OnDeath()
    {
        //reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    }
