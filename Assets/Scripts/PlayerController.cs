using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //goodplayer controlller
    [Header("Non-walk Properties")]
    public bool lastfaceright = true;
    public GameObject ground;
    public float gravity;
    public Vector3 Velocity;
    public float apexHeight;
    public float apexTime;
    public float jumpvel;
    public bool groundcheck;
    public float terminalvelocity;
    public float coyotetimer;
    public float coyotetime;
    public Boolean walljump;
    public Boolean walljumpcapacity;
    [Header("Walk properties")]
    public float acceleration;
    public float accelerationtime;
    public float deacceleration;
    public float deaccelerationtime;
    public float maxspeed;
    public float maxspeedtime;
    public float stoptime;
    public float runspeed;
    public float friction;
    public float dashtime = 1;
    public float truemaxspeed;
    Boolean dashing = false;
    public float dashingtime = 1.25f;

    //public bool IsTouching()

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = -2 * apexHeight / (apexTime*apexTime);
        jumpvel = 2 * apexHeight / apexTime;

        acceleration = maxspeed/accelerationtime;
        deacceleration = maxspeedtime/deaccelerationtime;
        truemaxspeed = maxspeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        //IsGrounded();
        //The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();


        if(Input.GetKeyDown(KeyCode.UpArrow) && (IsGrounded() == true))
        {
            playerInput.y = 1f;
            //Vector3 offset = new Vector3(0, .4f, 0);
            //transform.position += offset;
            //Jumpingmotion(playerInput);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && (IsGrounded() == false))
        {
            playerInput.y = 1f;
            //Debug.Log("Failed jump");
            //Jumpingmotion(playerInput);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput.x = -1f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput.x = 1f;
        }
        if (playerInput.y != 0) Debug.Log(playerInput.y);

        if (Input.GetKey(KeyCode.LeftArrow) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (dashtime == 1))
        {
            dashtime = dashingtime;
            //maxspeed = maxspeed * dashtime;
            Velocity.x = Velocity.x * 5;
            playerInput.x = -1f;
            dashing = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (dashtime == 1))
        {
            dashtime = dashingtime;
            //maxspeed = maxspeed * dashtime;
            Velocity.x = Velocity.x * 5;
            playerInput.x = 1f;
            dashing = true;
        }








        if (((Input.GetKey(KeyCode.LeftArrow) == false) && (Input.GetKey(KeyCode.RightArrow) == false)))
        {
            playerInput.x = 0;
        }

        //if(Velocity.y!=0)
        //{
        //    Debug.Log(Velocity.y);
        //}

        //Debug.Log(playerInput.x)
        //there was a ; right below here for whatever reason
        MovementUpdate(playerInput);
        if (Input.GetKeyDown(KeyCode.UpArrow) == false)
        {
            playerInput.y = 0;
        }

        if(dashtime>1) dashtime -= Time.deltaTime;
        if (dashtime < 1)
        {
            dashing = false;
            dashtime = 1;
            maxspeed = truemaxspeed;
        }
        


    }

        

    private void MovementUpdate(Vector2 playerInput)
    {
        if (IsGrounded() && playerInput.y == 1)
        {

            Velocity.y = jumpvel;
            //Debug.Log("Reference: " + jumpvel + " Against: " + Velocity.y);

        }
        else if (coyotetimer<coyotetime && playerInput.y == 1)
        {
            Velocity.y = jumpvel;
        }

        else if(!IsGrounded() && walljumpcapacity && walljump && playerInput.y==1)
        {
            Debug.Log("Wall JUMP!");
            Velocity.y = jumpvel;
            walljump = false;
        }

        else if (!IsGrounded())
        {
            //Debug.Log("Before: " + Velocity.y);


            Velocity.y += gravity * Time.deltaTime;


            //Debug.Log("After: " + Velocity.y);
            if (Velocity.y < terminalvelocity)
            {
                Velocity.y = terminalvelocity;
            }
        }

        //Problem Code
        else if (IsGrounded() && playerInput.y == 0 && Velocity.y < 0)
        {
            Velocity.y = 0f;
        }

        if (playerInput.x != 0)
        {
            if (Mathf.Sign(playerInput.x) != Mathf.Sign(Velocity.x)) Velocity.x *= -1;
            Velocity.x += acceleration * playerInput.x*Time.deltaTime;
            if (dashing == false) Velocity.x = Mathf.Clamp(Velocity.x, -maxspeed, maxspeed);
            
        }
        else if (playerInput.x == 0 && Velocity.x != 0)
        {
            //Velocity.x -= (friction * (Velocity.x / Mathf.Abs(Velocity.x)) * Time.deltaTime);
            Velocity.x += -Mathf.Sign(Velocity.x) * deacceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(Velocity.x) < 0.2f)
            {
                Velocity.x = 0;
            }
        }
        if (Velocity.x == float.NaN) Velocity.x = 0;

            transform.position += Velocity * Time.deltaTime;
        //    Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(playerInput, ForceMode2D.Force);
        

        }

    public bool IsWalking()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        PolygonCollider2D pb = GetComponent<PolygonCollider2D>();
        //if (rb.IsTouching(ground.GetComponent<CompositeCollider2D>()))
        if (pb.IsTouching(ground.GetComponent<CompositeCollider2D>()) && (rb.IsTouching(ground.GetComponent<CompositeCollider2D>())))
        {
            groundcheck = true;
            coyotetimer = 0;
            walljump = true;
            return true;
        }

        else if(!pb.IsTouching(ground.GetComponent<CompositeCollider2D>()) && (rb.IsTouching(ground.GetComponent<CompositeCollider2D>())))
        {
            //Debug.Log("touching a wall in the air");
            walljumpcapacity = true;
            groundcheck = false;
            return false;
        }
        else
        {
            groundcheck = false;
            coyotetimer += Time.deltaTime;
            return false;
        }

           
    }

    public FacingDirection GetFacingDirection()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //if ((rb.linearVelocity.x > 0) && rb.linearVelocity.x>.1f)
        //{
        //    lastfaceright = true;
        //    return FacingDirection.right;
        //}
        if (Velocity.x>0)
        {
            lastfaceright = true;
            return FacingDirection.right;
        }
        //else if ((rb.linearVelocity.x < 0) && rb.linearVelocity.x < -.1f)
        //{
        //    lastfaceright = false;
        //    return FacingDirection.left;
        //}
        else if (Velocity.x<0)
        {
            lastfaceright = false;
            return FacingDirection.left;
        }
        else
        {

           if(lastfaceright)
            {
                return FacingDirection.right;
            }

           else
            {
                return FacingDirection.left;
            }
        }

    }
}
