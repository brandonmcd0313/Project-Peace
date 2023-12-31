using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    float _minXPosition, _maxXPosition, _minYPosition, _maxYPosition;
    Vector3 _defaultScale;
    bool isGrounded;
    bool isFacingRight = true;
    bool canMove = true;
    bool canmovecooldown = false;
    Animator anim;


    [Header("Animations")]
    [SerializeField] AnimationClip _idleAnimation;
    [SerializeField] AnimationClip _walkAnimation;
    [SerializeField] AnimationClip _jumpAnimation;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _defaultScale = transform.localScale;
        _minXPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        _maxXPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;
        _minYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        _maxYPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;

        anim.SetBool(_idleAnimation.name, true);
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Reset"))
        {
            //reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (!canMove)
        {
            anim.SetBool(_jumpAnimation.name, false);
            anim.SetBool(_walkAnimation.name, false);
            anim.SetBool(_idleAnimation.name, true);
            return;
        }
        if (!isGrounded)
        {
            anim.SetBool(_jumpAnimation.name, true);
            anim.SetBool(_idleAnimation.name, false);
            anim.SetBool(_walkAnimation.name, false);
        }
        // Check if the player is jumping and play the jump animation
        else if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.Space)) && isGrounded)
        {
            anim.SetBool(_idleAnimation.name, false);
            anim.SetBool(_walkAnimation.name, false);
            anim.SetBool(_jumpAnimation.name, true);
        }
        // Check if the player is moving and play the walk animation
        else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && isGrounded)
        {
            anim.SetBool(_jumpAnimation.name, false);
            anim.SetBool(_idleAnimation.name, false);
            anim.SetBool(_walkAnimation.name, true);
        }
        else if(Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f)
        {
            anim.SetBool(_jumpAnimation.name, false);
            anim.SetBool(_walkAnimation.name, false);
            anim.SetBool(_idleAnimation.name, true);
        }
     

    }
    // Update is called once per frame

    bool canJump = true;
    //canmove jump cooldown
   IEnumerator CanMoveCooldown()
    {
        canJump = false;
        //wait until canmove is true
        yield return new WaitUntil(() => canMove == true);
        //wait until space bar is released
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        canJump = true;
        canmovecooldown = false;

    }
    void FixedUpdate()
    {
        if(!canMove && !canmovecooldown)
        {
            StartCoroutine(CanMoveCooldown());
            canmovecooldown = true;
        }
        if (!canMove)
        {
            anim.SetBool(_jumpAnimation.name, false);
            anim.SetBool(_walkAnimation.name, false);
            anim.SetBool(_idleAnimation.name, true);
            return;
        }
        
        
        float horizontalInput = Input.GetAxis("Horizontal");

        
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0);

        

        //set sprite direction based on mouse direction
        //if mouse is to the right of the player the player IS FACING RIGHT
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > transform.position.x)
        {
            isFacingRight = true;
            transform.localScale = _defaultScale;
        }
        else
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-_defaultScale.x, _defaultScale.y, _defaultScale.z);
        }


        if ((Input.GetButton("Jump")|| Input.GetKey(KeyCode.Space)) && isGrounded && canJump)
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
        if (collision.gameObject.tag == "Platform")
        {
            isGrounded = true;
            
        
        }
    }
    
    public bool isPlayerFacingRight()
    {
        return isFacingRight; 
    }

    public void SetMovement(bool b)
    {
        canMove = b;
    }
}
