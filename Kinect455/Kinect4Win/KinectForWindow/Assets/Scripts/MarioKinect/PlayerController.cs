using GameAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameActionManager actionManager;

    public float speed = 6.0f;
    public float jumpSpeedX = 8.0f;
    public float jumpSpeedY = 8.0f;

    private Rigidbody2D rb;
    private GroundDetect groundDetect;

    private Vector3 jumpVector = Vector3.up;
    private float moveDirection = 0;

    private bool previousGround;
    private bool prevJumped;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetect = GetComponentInChildren<GroundDetect>();
    }

    private void FixedUpdate()
    {
        previousGround = groundDetect.IsGrounded;
        moveDirection = 0;
        jump = false;
        ActionUpdate();
        //KeyboardUpdate();
        bool landing = (!previousGround && groundDetect.IsGrounded);
        if (jump && !groundDetect.IsGrounded) //in air
        {

        }
        else if (landing)
        {
            rb.velocity = Vector2.zero;
        }
        else if (jump && groundDetect.IsGrounded) // jumping
        {
            rb.AddForce(new Vector2(jumpSpeedX * jumpVector.x, jumpSpeedY * jumpVector.y));
        }


        if (!jump && groundDetect.IsGrounded) // moving
        {
            transform.Translate(new Vector2(speed * moveDirection * Time.deltaTime,0));
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        }
    }

    /*void Update()
    {
        if (!actionManager.isStreaming)
        {
            moveDirection = Input.GetAxis("Horizontal");
        }
        // rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

        // allowed to jump condition
        if (!prevJumped && groundDetect.IsGrounded && ( Input.GetAxis("Vertical") > 0 ||
            actionManager.gameAction == KinectAction.JUMP_LEFT||
            actionManager.gameAction == KinectAction.JUMP_RIGHT||
            actionManager.gameAction == KinectAction.JUMP_UP
            ))
        {
            prevJumped = true;
            rb.AddForce(new Vector2(jumpVector.x * jumpSpeedX, jumpVector.y * jumpSpeedY));
            //Debug.Log("fly");

        }
        else if (!previousGround && groundDetect.IsGrounded)
        {
            prevJumped = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (groundDetect.IsGrounded && !jump)
        {
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        }

        ActionUpdate();

        previousGround = groundDetect.IsGrounded;

    }*/
    bool jump;
    void Update()
    {
       
    }

    
    
    private void ActionUpdate()
    {

        if (actionManager.gameAction == KinectAction.STAND)
        {
            //Debug.Log("standing");
            moveDirection = 0;
        }
        else if (actionManager.gameAction == KinectAction.RUN_RIGHT)
        {
            Debug.Log("run right");
            moveDirection = 1;
        }
        else if (actionManager.gameAction == KinectAction.RUN_LEFT)
        {
            Debug.Log("run left");
            moveDirection = -1;
        }
        else if (actionManager.gameAction == KinectAction.JUMP_UP)
        {
            Debug.Log("jump up");
            jumpVector = Vector3.up;
            jump = true;
        }
        else if (actionManager.gameAction == KinectAction.JUMP_LEFT)
        {
            Debug.Log("jump left");
            jumpVector = new Vector3(-1, 1, 0);
            jump = true;
        }
        else if (actionManager.gameAction == KinectAction.JUMP_RIGHT)
        {
            Debug.Log("jump right");
            jumpVector = new Vector3(1, 1, 0);
            jump = true;
        }
        else if (actionManager.gameAction == KinectAction.STAND_ATTACK)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("stand attack");
        }
        else if (actionManager.gameAction == KinectAction.ATTACK_RIGHT)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack right");
        }
        else if (actionManager.gameAction == KinectAction.ATTACK_LEFT)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack left");
        }
        else
        {
            //playerRB.velocity = Vector3.zero;
            Debug.LogError("undefined action");
        }
    }

    private void KeyboardUpdate()
    {

        if (Input.GetKey(KeyCode.S))
        {
            //Debug.Log("standing");
            moveDirection = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("run right");
            moveDirection = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("run left");
            moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("jump up");
            jumpVector = Vector3.up;
            jump = true;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("jump left");
            jumpVector = new Vector3(-1, 1, 0);
            jump = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("jump right");
            jumpVector = new Vector3(1, 1, 0);
            jump = true;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("stand attack");
        }
        else if (Input.GetKey(KeyCode.C))
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack right");
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack left");
        }
        else
        {
            //playerRB.velocity = Vector3.zero;
            Debug.LogError("undefined action");
        }
    }

}
