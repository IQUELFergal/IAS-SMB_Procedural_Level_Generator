using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 5;
    public bool godMode = false;
    public bool isGrounded = false;
    public LayerMask mask;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.started)
            {
                //Debug.Log("Jump pushed down");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            /*if (context.performed)
            {
                Debug.Log("Jump pressed");
            }
            if (context.canceled)
            {
                Debug.Log("Jump released");
            }*/
        }
    }

    public void OnEnableGodModeInput(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            godMode = true;
        }
        if (context.canceled)
        {
            godMode = false;
        }
        UpdateCollisions(godMode);
    }

    private void Update()
    {
        CheckGround();
        
        if (godMode)
        {
            rb.velocity = movementInput * speed;
        }
        else rb.velocity = new Vector2(movementInput.x * speed, rb.velocity.y);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + 0.5f * Vector2.down, new Vector2(1, 0.05f), 0, mask);
    }

    void UpdateCollisions(bool mode)
    {
        if (mode)
        {
            col.enabled = false;
            rb.gravityScale = 0;
        }
        else
        {
            col.enabled = true;
            rb.gravityScale = 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else Gizmos.color = Color.red;

        Gizmos.DrawCube((Vector2)transform.position + 0.5f * Vector2.down, new Vector2(1, 0.05f));
    }
}
