using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private SpriteRenderer spr;

    [SerializeField] private GameObject bombPrefab;

    [SerializeField] private float shootRange = 7f;

    [SerializeField] private float throwStrength = 5f;
    private Vector2 playerDirection;

    private bool botTurn = false;

    public static EnemyScript instance;

    private float distanceFromPlayer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        TurnBasedSystem.onTurnStarted += startTurn;
        TurnBasedSystem.onTurnFinished += endTurn;
        TurnBasedSystem.onChoiceStarted += startChoice;
    }

    // Update is called once per frame
    void Update()
    {



        if (!isGrounded())
        {
            anim.SetBool("jumping", true);
        }

        if (rb.velocity.y < 0)
        {
            anim.SetBool("falling", true);
        }

        if (rb.velocity.x != 0)
        {
            anim.SetBool("walking",true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if(rb.velocity.x > 0)
        {
            spr.flipX = false;
        }
        else if(rb.velocity.x < 0)
        {
            spr.flipX = true;
        }

        if (isGrounded())
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
        }
    }

    private void FixedUpdate()
    {
        if (botTurn)
        {
            moveTowardsPlayer();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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

    private bool shouldJump()
    {
        return Physics2D.Raycast(transform.position, transform.right, 1.2f, groundMask) || Physics2D.Raycast(transform.position, -transform.right, 1.2f, groundMask);
    }

    private void moveTowardsPlayer()
    {
        playerDirection = PlayerController.instance.transform.position - transform.position;

        if(shouldJump() && isGrounded())
        {
            Jump();
        }

        rb.velocity = new Vector2(playerDirection.normalized.x * speed * Time.fixedDeltaTime * 100,rb.velocity.y);
    }


    private void startTurn(string currentTurn, int rollSize)
    {
        if (currentTurn == "bot")
        {
            botTurn = true;
        }

    }

    private void endTurn(string currentTurn, int rollSize)
    {
        if (currentTurn == "bot")
        {
            //TODO:...
        }

    }

    private void startChoice(string currentTurn, int rollSize)
    {
        if (currentTurn == "bot")
        {
            distanceFromPlayer = Vector2.Distance(transform.position, PlayerController.instance.transform.position);

            botTurn = false;

            chooseAction();
        }
    }

    private void chooseAction()
    {
        if (distanceFromPlayer <= 1f)
        {
            hit();
        }
        else if (distanceFromPlayer <= shootRange)
        {
            shoot();
        }

        TurnBasedSystem.insance.endTurn("bot");
    }


    private void shoot()
    {
        Debug.Log("Bomb shot");
        GameObject b = Instantiate(bombPrefab, (Vector2)transform.position + Vector2.up * 2, bombPrefab.transform.rotation);
        b.GetComponent<Rigidbody2D>().AddForce((playerDirection + Vector2.up ) * (throwStrength + distanceFromPlayer), ForceMode2D.Impulse);
    }

    private void hit()
    {
        PlayerController.instance.TakeDamage(3);
    }
}
