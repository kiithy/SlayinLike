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
    public float doubleJumpSpeed;
    private bool faceRightState = true;
    public Animator knightAnimator;
    private bool jumpedState = false;
    private bool onGroundState = true;
    private bool alive = true;
    private bool moving = false;
    private bool damaged = false;
    private bool invincible = false;
    private int attackCombo = 0;
    private float comboTimer = 0;
    private float comboTimeWindow = 0.5f;

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
            knightAnimator.SetBool("Moving", moving);
            knightBody.velocity = new Vector2(0, knightBody.velocity.y);
        }
        else{
            FlipKnightSprite(value);
            moving = true;
            knightAnimator.SetBool("Moving", moving);
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
            knightBody.AddForce(Vector2.up * upSpeed * (doubleJumpSpeed * 10), ForceMode2D.Force);
            jumpedState = false;
        }
    }

    public void Attack()
    {
        if (alive)
        {
            if (comboTimer > 0 && attackCombo == 1)
            {
                attackCombo = 2;
            }
            else
            {
                attackCombo = 1;
            }

            knightAnimator.SetInteger("AttackType", attackCombo);
            knightAnimator.SetTrigger("Attack");

            comboTimer = comboTimeWindow; // Reset combo timer
        }
    }

    private void UpdateComboTimer()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            attackCombo = 0;
            knightAnimator.SetInteger("AttackType", attackCombo);
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
        UpdateComboTimer();
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && !invincible)
        {
            // SECTION - Knight is damaged
            invincible = true;
            Debug.Log("Knight is damaged");
            damaged = true;
            knightAnimator.SetBool("damaged", damaged);
            StartCoroutine(Damaged());
        }
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(2);
        damaged = false;
        knightAnimator.SetBool("damaged", damaged);
        invincible = false;
    }
}
