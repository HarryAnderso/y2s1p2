using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();

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
        //Debug.Log(playerInput.x)
;        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(playerInput, ForceMode2D.Force);
        

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
        return false;
    }

    public FacingDirection GetFacingDirection()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if ((rb.linearVelocity.x > 0) && rb.linearVelocity.x>.1f)
        {
            return FacingDirection.right;
        }
        else if ((rb.linearVelocity.x < 0) && rb.linearVelocity.x < -.1f)
        {
            return FacingDirection.left;
        }
        else
        {
            return FacingDirection.right;
        }

    }
}
