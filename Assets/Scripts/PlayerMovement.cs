using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Player
{
    private Vector2 moveInput;

    protected override void Update()
    {
        base.Update();

        if (!isAlive) return;

        Run();
        FlipBody();
    }

    public void OnMove(InputValue value)
    {
        if (!isAlive) return;

        if (!isSitting)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    public void OnSit(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed)
        {
            isSitting = true;
            moveInput = Vector2.zero;

        }
        else
        {
            isSitting = false;
            isStanding = true;
        }
    }

    public void OnStandUp(InputValue value)
    {
        if (!isAlive) return;

        if (!value.isPressed && !isSitting)
        {
            isStanding = true;
        }
        else
        {
            isStanding = false;
        }
    }

    public void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        if (value.isPressed && !isSitting)
        {
            myRigidbody2D.velocity += new Vector2(0f, jumpAmount);
            myAnimator.SetBool("isJumping", true);
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isWalking", false);
        }
    }

    private void Run()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        Vector2 playerVelocity = new Vector2(speed * moveInput.x, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, -Vector2.up, 1.5f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), -Vector2.up, 1.5f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), -Vector2.up, 1.5f, LayerMask.GetMask("Ground"));

        bool falling = hitCenter.collider == null && hitLeft.collider == null && hitRight.collider == null;

        myAnimator.SetBool("isJumping", falling);
        myAnimator.SetBool("isRunning", hasHorizontalSpeed && !falling && isRunning);
        myAnimator.SetBool("isWalking", hasHorizontalSpeed && !falling && !isRunning);
        myAnimator.SetBool("isSitting", isSitting);
        myAnimator.SetBool("isStanding", isStanding && !isSitting);
    }

    private void FlipBody()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }
}
