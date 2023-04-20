using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpStrength = 5;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private SpriteRenderer spr;

    [SerializeField] private float health = 5;

    [SerializeField] private float shootRange = 7f;

    public static PlayerController instance;


    private bool movementEnabled = false;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        TurnBasedSystem.onTurnStarted += startTurn;
        TurnBasedSystem.onTurnFinished += endTurn;
        TurnBasedSystem.onChoiceStarted += startChoice;
    }

    private void Update()
    {
        if (movementEnabled)
        {

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            {
                Jump();
            }
            else if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y >= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        
        if (!isGrounded())
        {
            anim.SetBool("jumping", true);
        }

        if (rb.velocity.y < 0)
        {
            anim.SetBool("falling", true);
        }

        if (isGrounded())
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
        }

    }

    void FixedUpdate()
    {
        if (movementEnabled)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveX * speed * Time.fixedDeltaTime * 100, rb.velocity.y);

            if (moveX > 0)
            {
                spr.flipX = false;
            }
            else if (moveX < 0)
            {
                spr.flipX = true;
            }

            if (moveX != 0)
            {
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }
        }
        else
        {
            anim.SetBool("walking", false);
            rb.velocity = new Vector2(0,rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundMask);
    }

    private void startTurn(string currentTurn, int rollSize)
    {
        if (currentTurn == "player")
        {
            movementEnabled = true;
        }
        
    }

    private void endTurn(string currentTurn, int rollSize)
    {
        if (currentTurn == "player")
        {
            //TODO:...
        }

    }

    private void startChoice(string currentTurn, int rollSize)
    {
        if(currentTurn == "player")
        {
            movementEnabled = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

}
