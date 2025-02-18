using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameplay : MonoBehaviour
{
    private KnightActions controls;
    private Vector2 moveInput;
    private bool isJumping;
    private bool isJumpHolding;
    private Rigidbody2D knightBody;
    private SpriteRenderer knightSprite;
    public float maxSpeed = 10;
    public float speed = 5;
    public float upSpeed = 4;
    private bool faceRightState = true;
    public Animator knightAnimator;
    private bool jumpedState = false;
    private bool onGroundState = true;
    private bool alive = true;
    private bool moving = true;

    private void Awake()
    {
        controls = new KnightActions();
    }


    void Move(int value)
    {
        Vector2 movement = new Vector2(value * speed, knightBody.velocity.y);
        knightBody.velocity = movement;
    }

    void FlipKnightSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            knightSprite.flipX = true;
            Debug.Log("Flip to left");
            // if (knightBody.velocity.x > 0.05f)
            //     knightAnimator.SetTrigger("onSkid");

        } else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            knightSprite.flipX = false;
            Debug.Log("Flip to right");
            // if (knightBody.velocity.x > 0.05f)
            //     knightAnimator.SetTrigger("onSkid");
        }
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
            knightBody.velocity = new Vector2(0, knightBody.velocity.y);
        }
        else{
            FlipKnightSprite(value);
            moving = true;
            Move(value);
        }

    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            knightBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            knightAnimator.SetBool("OnGroundState", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            knightBody.AddForce(Vector2.up * upSpeed * 10, ForceMode2D.Force);
            jumpedState = false;
        }
    }

    private void OnJumpHold(InputAction.CallbackContext context)
    {
        isJumpHolding = context.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        knightBody = GetComponent<Rigidbody2D>();

        knightSprite = GetComponent<SpriteRenderer>();
        knightAnimator.SetBool("OnGroundState", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        knightAnimator.SetFloat("xSpeed", Mathf.Abs(knightBody.velocity.x));
    }


    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("OnGround");
            onGroundState = true;
            knightAnimator.SetBool("OnGroundState", onGroundState);
        }
    }
}
