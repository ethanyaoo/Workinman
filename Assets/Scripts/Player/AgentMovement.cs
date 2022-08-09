using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class AgentMovement : MonoBehaviour
{
    protected Vector2 moveInput;

    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] GameObject bomb;
    [SerializeField] Transform throwPosition;


    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpVelocity = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] Vector2 deathVelocity = new Vector2(5f, 5f);


    float gravityScaleAtStart;
    int numJumps = 0;

    bool isAlive = true;
    private bool isPlayerMovementDisabled = false;
    private Vector2 currVelocity;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = rigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMovementDisabled)
        {
            return;
        }

        if (!isAlive)
        {
            return;
        }

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            setJumpingAnimation(new Vector2(0f, 0f));
            numJumps = 0;
        }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    /// <summary>
    /// Flipping sprite based on rigidbody velocity x and flipping. Using epsilon as using 0 sometimes causes problems
    /// </summary>
    public void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2d.velocity.x), 1f);
        }
    }

    /// <summary>
    /// Listens to call for movements in input mapping for movement
    /// </summary>
    ///
    void OnMove(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }

        moveInput = inputValue.Get<Vector2>();
    }

    /// <summary>
    /// Listens for jump call and checks colliders to make sure legal jump
    /// </summary>
    void OnJump(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }

        if (numJumps == 1 && !boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        else if (inputValue.isPressed)
        {
            rigidbody2d.velocity += new Vector2(0f, jumpVelocity);
            setJumpingAnimation(rigidbody2d.velocity);
            numJumps++;
        }
    }

    /// <summary>
    /// Listens for fire call
    /// </summary>
    void OnFire(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }

        setThrowingAnimation(true);

        StartCoroutine(ThrowObject());
    }

    /// <summary>
    /// Only need to change x velocity
    /// </summary>
    public void Run()
    {
        Vector2 runVelocity = new Vector2(runSpeed * moveInput.x, rigidbody2d.velocity.y);
        rigidbody2d.velocity = runVelocity;
        setRunningAnimation(runVelocity);
    }

    /// <summary>
    /// Checks to see if player is touching capsule collider for climbing tilemap layer
    /// turns off gravity to remove falling while climbing
    /// Invoke animation function and set to climb
    /// </summary>
    public void ClimbLadder()
    {
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rigidbody2d.velocity.x, moveInput.y * climbSpeed);
        rigidbody2d.velocity = climbVelocity;
        rigidbody2d.gravityScale = 0f;
        setClimbingAnimation(climbVelocity);
    }

    private IEnumerator ThrowObject()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        Instantiate(bomb, throwPosition.position, transform.rotation);

        // Waits for throw animation to be over
        yield return new WaitForSecondsRealtime(0.5f);

        setThrowingAnimation(false);
    }

    /// <summary>
    /// Death method
    /// </summary>
    /// 
    void Die()
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            setDeathAnimation();

            rigidbody2d.velocity = deathVelocity;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    /// <summary>
    /// Enable player movement
    /// </summary>
    /// 
    public void EnablePlayer()
    {
        isPlayerMovementDisabled = false;
    }

    /// <summary>
    /// Disable player movement
    /// </summary>
    /// 
    public void DisablePlayer()
    {
        isPlayerMovementDisabled = true;

        currVelocity = rigidbody2d.velocity;
        rigidbody2d.velocity = new Vector2(0f, 0f);
    }


    /// <summary>
    /// Animation Functions
    /// </summary>
    /// 

    public void setRunningAnimation(Vector2 movementVelocity)
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(movementVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    public void setClimbingAnimation(Vector2 movementVelocity)
    {
        bool playerHasVerticalSpeed = Mathf.Abs(movementVelocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);

        animator.SetBool("isJumping", false);
    }

    public void setJumpingAnimation(Vector2 movementVelocity)
    {
        bool playerHasVerticalSpeed = Mathf.Abs(movementVelocity.y) > Mathf.Epsilon;

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", playerHasVerticalSpeed);
        }
    }

    public void setThrowingAnimation(bool val)
    {
        animator.SetBool("isThrowing", val);
    }

    public void setDeathAnimation()
    {
        Debug.Log("dead");
        animator.SetBool("isDead", true);
    }
}
