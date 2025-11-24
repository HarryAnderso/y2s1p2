using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    public float runspeed;
    public float friction;

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
            //playerInput.y = 1f;
            Debug.Log("Failed jump");
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
        else if (IsGrounded() && playerInput.y ==0 && Velocity.y<0)
        { 
            Velocity.y = 0f;
        }

        if (playerInput.x != 0)
        {
            Velocity.x = runspeed * playerInput.x;
        }
        else if (playerInput.x == 0 && Velocity.x != 0)
        {
            Velocity.x -= (friction * (Velocity.x / Mathf.Abs(Velocity.x)) * Time.deltaTime);

            if (Mathf.Abs(Velocity.x) < 0.2f)
            {
                Velocity.x = 0;
            }
        }
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
        if (rb.IsTouching(ground.GetComponent<CompositeCollider2D>()))
        {
            groundcheck = true;
            coyotetimer = 0;
            return true;
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
