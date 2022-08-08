using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimations : MonoBehaviour
{
    [SerializeField] protected Animator agentAnimator;
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        agentAnimator = GetComponent<Animator>();
    }

    public void setRunningAnimation(bool val)
    {
        agentAnimator.SetBool("isRunning", val);

        agentAnimator.SetBool("isJumping", false);
        agentAnimator.SetBool("isClimbing", false);
    }

    public void setJumpingAnimation(bool val)
    {
        agentAnimator.SetBool("isJumping", val);

        agentAnimator.SetBool("isRunning", false);
        agentAnimator.SetBool("isClimbing", false);
    }

    public void setClimbingAnimation(bool val)
    {
        agentAnimator.SetBool("isClimbing", val);

        agentAnimator.SetBool("isJumping", false);
        agentAnimator.SetBool("isRunning", false);
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

    public void animatePlayerReset(Vector2 movementVelocity, Vector2 moveInput)
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(movementVelocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(movementVelocity.y) > Mathf.Epsilon;

        if (!playerHasVerticalSpeed)
        {
            setJumpingAnimation(false);
        }

        if (!playerHasHorizontalSpeed)
        {
            setRunningAnimation(false);
        }

        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            setClimbingAnimation(false);
        }
    }

    public void animatePlayerMovement(Vector2 movementVelocity, Vector2 moveInput, string moveAction)
    {
        FlipSprite();

        bool playerHasHorizontalSpeed = Mathf.Abs(movementVelocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(movementVelocity.y) > Mathf.Epsilon;
        
        switch (moveAction)
        {
            case "climb":
                if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
                {
                    setClimbingAnimation(playerHasVerticalSpeed);
                }
                break;

            case "run":
                setRunningAnimation(playerHasHorizontalSpeed);
                break;

            case "jump":
                setJumpingAnimation(playerHasVerticalSpeed);
                break;
        }
    }
}
