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
    public Vector2 bouncevector;
    public Boolean bounced;
    public float bouncetimer;
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
        //uses the public floats to determine jump properties
        gravity = -2 * apexHeight / (apexTime*apexTime);
        jumpvel = 2 * apexHeight / apexTime;
        //same but for horizontal movement
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

        //basic jump from the ground
        if(Input.GetKeyDown(KeyCode.UpArrow) && (IsGrounded() == true))
        {
            playerInput.y = 1f;
        }
        //for when the player jumps while not grounded
        if (Input.GetKeyDown(KeyCode.UpArrow) && (IsGrounded() == false))
        {
            playerInput.y = 1f;
        }
        //for when the player moves left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput.x = -1f;
        }
        //for when the player moves right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput.x = 1f;
        }
        //checks for when the player wishes to dash left
        if (Input.GetKey(KeyCode.LeftArrow) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (dashtime == 1))
        {
            //normally dashtime is equal to 1, dashing time determines for long long the player gets incresed movement
            dashtime = dashingtime;
            //maxspeed = maxspeed * dashtime;
            Velocity.x = Velocity.x * 5;
            playerInput.x = -1f;
            dashing = true;
        }
        //for when the player wants to dash right
        if (Input.GetKey(KeyCode.RightArrow) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (dashtime == 1))
        {
            //normally dashtime is equal to 1, dashing time determines for long long the player gets incresed movement
            dashtime = dashingtime;
            //maxspeed = maxspeed * dashtime;
            Velocity.x = Velocity.x * 5;
            playerInput.x = 1f;
            dashing = true;
        }







        //resets player horizontal input
        if (((Input.GetKey(KeyCode.LeftArrow) == false) && (Input.GetKey(KeyCode.RightArrow) == false)))
        {
            playerInput.x = 0;
        }

        //calls the movement update with any changes in player input
        MovementUpdate(playerInput);
        //resets player verticle input
        if (Input.GetKeyDown(KeyCode.UpArrow) == false)
        {
            playerInput.y = 0;
        }
        //counts down dashtime by time passed between each frame
        if(dashtime>1) dashtime -= Time.deltaTime;
        //resets dash time
        if (dashtime < 1)
        {
            dashing = false;
            dashtime = 1;
            maxspeed = truemaxspeed;
        }
        
        //resets player gravity when the player has already bounced
        if(!bounced)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
       //used to reset the bounce vector amounts
        if(bounced)
        {
            bouncevector = Vector3.zero;
        }
        //starts a cooldown for the bounce timer. this is nessecary to ensure the player doesnt bounce infinitly
        if (bounced && IsGrounded())
        {
            bouncetimer += Time.deltaTime;
        }
        // resets the bounce timer if the player is no longer grounded
        if (bounced && !IsGrounded())
        {
            bouncetimer = 0;
        }
        //restes the bounce timer if the player has stayed grounded for long enough. while this should technicly be determined by a public variable, any number lower then this has led to issues with infinite bouncing, hence why its a static number
        if (bouncetimer > .75f)
        {
            bounced = false;
            bouncetimer = 0;
        }

    }

        

    private void MovementUpdate(Vector2 playerInput)
    {
        //interperts a jump
        if (IsGrounded() && playerInput.y == 1)
        {
            Velocity.y = jumpvel;
        }
        //allows for a jump during coyote time
        else if (coyotetimer<coyotetime && playerInput.y == 1)
        {
            Velocity.y = jumpvel;
        }
        //checks that the player is not grounded, within the vacinity of a wall, has not done a wall jump so far, and the player is inputting a jump key
        else if(!IsGrounded() && walljumpcapacity && walljump && playerInput.y==1)
        {
            Velocity.y = jumpvel;
            //prevents a player from preforming multiple wall jumps without touching the ground
            walljump = false;
        }

        else if (!IsGrounded())
        {
            //downwards movement acting against a airborne player
            Velocity.y += gravity * Time.deltaTime;
            if (Velocity.y < terminalvelocity)
            {
                //sets the downwards movement of the player to the terminal velocity
                Velocity.y = terminalvelocity;
            }
        }

        else if (IsGrounded() && playerInput.y == 0 && Velocity.y < 0)
        {
            //checks if the player has bounced, and is going fast enough to bounce
            if(Velocity.y<(-terminalvelocity/2) && !bounced)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                bouncevector = Velocity;
                bouncevector.y = (bouncevector.y/2) * -1;
                //before I added this line, you could stack dashing and bouncing to go crazy fast horizontally
                bouncevector.x = 0;
                //prevents the player from just resuming their downwards momentum
                Velocity.y = 0;
                rb.AddForce(bouncevector, ForceMode2D.Impulse);
                //prevents further bouncing until the timer is completed
                bounced = true;

            }

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
        //I dont entierly know if this code works, but it was a attempt at fixing a issue I had with early movement
        if (Velocity.x == float.NaN) Velocity.x = 0;
        //updates the position of the player 
            transform.position += Velocity * Time.deltaTime;
        

        }

    public bool IsWalking()
    {
        //next two if statements check if the player is moving for the animator
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        //tells the animator the player is currently idle
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        //the rigidbody  encompases the whole body, while the poly collider only encompases the feet, so it can check for groundesness without interacting with walls/roofs
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
        //this if statement checks to see if a wall jump is possible
        else if(!pb.IsTouching(ground.GetComponent<CompositeCollider2D>()) && (rb.IsTouching(ground.GetComponent<CompositeCollider2D>())))
        {
         
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
    //this code just checks what direction the player is facing based on velocity
    public FacingDirection GetFacingDirection()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Velocity.x>0)
        {
            lastfaceright = true;
            return FacingDirection.right;
        }
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
