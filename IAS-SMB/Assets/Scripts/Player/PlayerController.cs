using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float speed = 5;
    private float currentSpeed;
    [SerializeField] bool godMode = false;
    [SerializeField] Vector2 movementInput;
    [SerializeField] float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;
    [SerializeField] float lastMove = 1;

    [Header("Ground detection settings")]
    [SerializeField] Vector2 groundBoxSize = Vector3.one;
    [SerializeField] Vector2 groundBoxOffset = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded = false;
    private bool wasGrounded = false;

    [Header("Jump settings")]
    [SerializeField] float baseJumpHeight = 1;
    [SerializeField] float extraChargeJumpHeight = 2;
    [SerializeField] float maxJumpChargeTime = 3;
    [SerializeField] float coyoteTime = 0.2f;
    private float currentCoyoteTimeLeft = 0;
    private bool canJump = false;
    private bool instantJump = false;
    private bool chargingJump = false;
    private bool jumpWasCharging = false;
    [Range(0, 1)]
    [SerializeField] float airControlPercent = 0.4f;
    private float jumpChargeTime = 0;

    [SerializeField] float fallMultiplier = 2;

    [Header("Head collision settings")]
    [SerializeField] Vector2 headBoxSize = Vector3.one;
    [SerializeField] Vector2 headBoxOffset = Vector3.zero;
    [SerializeField] LayerMask headMask;

    [Header("Particles effects")]
    [SerializeField] ParticleSystem footstep;
    private ParticleSystem.EmissionModule footEmission;
    [SerializeField] ParticleSystem impactEffect;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Events")]
    
    [SerializeField] GameEvent OnPlayerHealthUpEvent;
    [SerializeField] GameEvent OnPlayerDeathEvent;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        footEmission = footstep.emission;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is TapInteraction)
            {
                instantJump = true;
            }
            if (context.interaction is HoldInteraction)
            {
                chargingJump = true;
            }
        }
        else if (context.canceled && chargingJump)
        {
            chargingJump = false;
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
        HandleCollisions(godMode);
    }

    private void Update()
    {
        //Grounding
        CheckGround();
        if (isGrounded) currentCoyoteTimeLeft = coyoteTime;
        else currentCoyoteTimeLeft -= Time.deltaTime;

        canJump = isGrounded || currentCoyoteTimeLeft > 0;

        //Footstep particles
        if (movementInput.x != 0 && isGrounded)
        {
            footEmission.rateOverTime = 35f;
        }
        else
        {
            footEmission.rateOverTime = 0;
        }

        //Movement
        if (godMode)
        {
            rb.velocity = movementInput * speed;
        }
        else
        {
            HandleMovement();
            HandleJump();
        }
        CheckHeadCollision();

        //Animations
        if (movementInput!=Vector2.zero) anim.SetBool("Running", true);
        else anim.SetBool("Running", false);

        if (lastMove < 0) sr.flipX = true;
        else sr.flipX = false;

        if(movementInput.x != 0) lastMove = movementInput.x;


        //Jump impact particles
        if (!wasGrounded && isGrounded)
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.Play();
        }

        wasGrounded = isGrounded;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + groundBoxOffset, groundBoxSize, 0, groundMask);
    }

    void CheckHeadCollision()
    {
        Collider2D col = Physics2D.OverlapBox((Vector2)transform.position + headBoxOffset, headBoxSize, 0, headMask);
        if (col)
        {
            /*Block block = col.gameObject.GetComponent<Block>();
            Debug.Log(col.gameObject.name);
            if (block != null)
            {
                Debug.Log("Block");
                block.BreakBlock();
            }*/
        }
    }

    void HandleCollisions(bool mode)
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


    void HandleMovement()
    {
        float targetSpeed = speed * movementInput.x;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        currentSpeed = rb.velocity.x;
    }

    void HandleJump()
    {
        if (canJump)
        {
            if (instantJump)
            {
                Jump(0);
                instantJump = false;
            }
            else if (chargingJump)
            {
                jumpChargeTime += Time.deltaTime;
                jumpWasCharging = true;

                if (jumpChargeTime > maxJumpChargeTime)
                {
                    jumpChargeTime = maxJumpChargeTime;
                    chargingJump = false;
                }
            }
            else if (jumpWasCharging && !chargingJump)
            {
                Jump(jumpChargeTime);
                jumpChargeTime = 0;
                jumpWasCharging = false;
            }
        }
    }

    void Jump(float chargedTime)
    {
        float jumpVelocity = Mathf.Sqrt(-2 * -10 * (baseJumpHeight + (chargedTime / maxJumpChargeTime * extraChargeJumpHeight)));
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Coin":
                Debug.Log("Coin");
                break;
            case "Enemy":
                Debug.Log("Enemy");
                break;
            default:
                Debug.Log("Collision with unknown gameObject");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else Gizmos.color = Color.red;
        //Foot box
        Gizmos.DrawCube((Vector2)transform.position + groundBoxOffset, groundBoxSize);
        Gizmos.color = Color.red;
        //Head box
        Gizmos.DrawCube((Vector2)transform.position + headBoxOffset, headBoxSize);
    }
}
