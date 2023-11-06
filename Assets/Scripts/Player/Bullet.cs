using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int bulletDamage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThisObject", 1f);
    }

    
    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            return;
        }

        //try to damage the object
        if (collision.gameObject.TryGetComponent(out IDamageable damageableObject))
        {
            damageableObject.Damage(bulletDamage);
        }

        //destroy this object
        Destroy(gameObject);
    }
}
