using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneController : MonoBehaviour
{
    public Action OnPlayerExitScreenSpaceRight;
    public Action OnPlayerExitScreenSpaceLeft;


    float minXPosition;
    float maxXPosition;
    // Start is called before the first frame update
    void Start()
    {
        //set the max and min x positions to the camera bounds
        minXPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        maxXPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        OnPlayerExitScreenSpaceRight += OnExitSceneFromTheRight;
        OnPlayerExitScreenSpaceLeft += OnExitSceneFromTheLeft;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //if on the right side of the screen and moving right
        if (transform.position.x >( maxXPosition - 0.5f)&& horizontalInput > 0)
        {
            //call the event
            OnPlayerExitScreenSpaceRight?.Invoke();
        }

        //if on the left side of the screen and moving left
        if (transform.position.x < (minXPosition + 0.5f) && horizontalInput < 0)
        {
            //call the event
            OnPlayerExitScreenSpaceLeft?.Invoke();
        }

        //clamp the player position to the camera bounds
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minXPosition, maxXPosition), transform.position.y);
    }

    void OnExitSceneFromTheRight()
    {
        print("OnExitSceneFromTheRight");
    }

    void OnExitSceneFromTheLeft()
    {
        print("OnExitSceneFromTheLeft");
    }
}
