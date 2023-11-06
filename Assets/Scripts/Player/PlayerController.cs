using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    float _minXPosition, _maxXPosition, _minYPosition, _maxYPosition;
    Vector3 _defaultScale;
    bool isGrounded;
    bool isFacingRight;
    // Start is called before the first frame update
    void Start()
    {
        _defaultScale = transform.localScale;
        _minXPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        _maxXPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;
        _minYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        _maxYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            //reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0);


        //set sprite direction based on input
        if (horizontalInput > 0)
        {
            isFacingRight = true;
            transform.localScale = _defaultScale;
        }
        else if (horizontalInput < 0)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-_defaultScale.x, _defaultScale.y, _defaultScale.z);
        }

        
        if ((Input.GetButtonDown("Jump")|| (Input.GetKeyDown(KeyCode.Space)) && isGrounded))
        {
            isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        
        }
        
        //if goes offscreen lock back on screen
        CheckScreenPosition();

       

    }

    void CheckScreenPosition()
    {
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
        if(collision.gameObject.tag == "Platform")
        {
            isGrounded = true;
        }
    }

    

    public bool isPlayerFacingRight()
    {
        return isFacingRight; 
    }
}
