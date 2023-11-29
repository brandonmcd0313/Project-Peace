using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    //health resets when a new part is entered
    [SerializeField] int _health;
    [SerializeField] AudioClip _hurtSound;
    public int Health { get; set; }
    AudioSource aud;
    void Start()
    {
        Health = _health;
        aud = GetComponent<AudioSource>();
    }
    public void Damage(int damage)
    {
        Health -= damage;
        aud.PlayOneShot(_hurtSound);
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
