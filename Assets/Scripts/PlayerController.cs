using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    float _minXPosition;
    float _maxXPosition;
    float _minYPosition;
    float _maxYPosition;
    // Start is called before the first frame update
    void Start()
    {
        _minXPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        _maxXPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;
        _minYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        _maxYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            OnPlayerDeath();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //movement on x and y axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0f);

        //if no input stop all movement
        if (horizontalInput == 0 && verticalInput == 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
        //rotate to be in the direction of the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //if goes offscreen lock back on screen
        
        if (transform.position.x < _minXPosition)
        {
            transform.position = new Vector3(_minXPosition, transform.position.y, transform.position.z);
        }
        if (transform.position.x > _maxXPosition)
        {
            transform.position = new Vector3(_maxXPosition, transform.position.y, transform.position.z);
        }
        if (transform.position.y < _minYPosition)
        {
            transform.position = new Vector3(transform.position.x, _minYPosition, transform.position.z);
        }
        if (transform.position.y > _maxYPosition)
        {
            transform.position = new Vector3(transform.position.x, _maxYPosition, transform.position.z);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collides with enemy restart level
        if (collision.gameObject.tag == "Enemy")
        {
            OnPlayerDeath();
           
        }
    }

    void OnPlayerDeath()
    {
        //restart anim here
        //reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
