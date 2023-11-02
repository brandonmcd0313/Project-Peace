using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour, IEnemy
{
    private GameObject _player;
    [SerializeField] float speed;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
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
        }
    }

    public void Move()
    {
        //move towards the player
        Vector3 lookDir = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        transform.position += lookDir.normalized * Time.deltaTime * speed;
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
