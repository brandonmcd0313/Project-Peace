using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpAndDown : MonoBehaviour
{
    
    [SerializeField] float speed;
    bool isDead = false;
    bool isMovingUp = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {

            return;
        }

        Move();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            OnDeath();
            return;
        }

        //swap the direction on movement
        isMovingUp = !isMovingUp;
        

    }

    public void Move()
    {
       if(isMovingUp)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        
         }
       else
         {
            transform.position -= Vector3.up * Time.deltaTime * speed;
        }

        transform.rotation = Quaternion.identity;
    }

    public void OnDeath()
    {
      
        isDead = true;
        gameObject.tag = "Dead";
        //change to dead sprite
        GetComponent<SpriteRenderer>().color = Color.red;
        //remove the rigidbody
        Destroy(GetComponent<Rigidbody2D>());
    }
}
