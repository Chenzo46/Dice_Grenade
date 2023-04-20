using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask wall;
    [SerializeField] private GameObject dice;
    [SerializeField] private Transform throwIndicator;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip noMoveSound;
    private DiceRoller currentDice;

    private Vector2 setDir = Vector2.right;
    private bool diceOut = false;

    private void OnEnable()
    {
        DiceRoller.onRollHitWall += resetRoll;
    }

    // Update is called once per frame
    void Update()
    {
        //DiceRolling
        if (Input.GetKeyDown(KeyCode.Space) && !diceOut && canPlace())
        {
            currentDice = Instantiate(dice, throwIndicator.position, dice.transform.rotation).GetComponent<DiceRoller>();
            currentDice.setDirection(setDir);
            diceOut = true;
        }

        //Indicator Placement

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            setDir = transform.up;
            throwIndicator.eulerAngles = new Vector3(0,0,90);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            setDir = -transform.right;
            throwIndicator.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            setDir = -transform.up;
            throwIndicator.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            setDir = transform.right;
            throwIndicator.eulerAngles = new Vector3(0, 0, 0);
        }

        throwIndicator.localPosition = setDir;

        //Movement
        if (canMove())
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                dir = Vector2.up;
                rb.MovePosition((Vector2)transform.position + dir);
                AudioManager.instance.playSound(moveSound);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                dir = Vector2.left;
                rb.MovePosition((Vector2)transform.position + dir);
                AudioManager.instance.playSound(moveSound);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                dir = Vector2.down;
                rb.MovePosition((Vector2)transform.position + dir);
                AudioManager.instance.playSound(moveSound);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                dir = Vector2.right;
                rb.MovePosition((Vector2)transform.position + dir);
                AudioManager.instance.playSound(moveSound);
            }
        }
        else if (!canMove() && !diceOut)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                AudioManager.instance.playSound(noMoveSound);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                AudioManager.instance.playSound(noMoveSound);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                AudioManager.instance.playSound(noMoveSound);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                AudioManager.instance.playSound(noMoveSound);
            }
        }

    }
    
    private bool canPlace()
    {
        return !Physics2D.Raycast(transform.position, setDir, 1f, wall);
    }

    private bool canMove()
    {
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        return !Physics2D.Raycast(transform.position, moveDir, 1f, wall) && !diceOut;
    }

    private void resetRoll()
    {
        diceOut = false;
        currentDice = null;
    }
}
