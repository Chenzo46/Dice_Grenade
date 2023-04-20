using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask destroyObjs;
    [SerializeField] private Sprite[] sprites = new Sprite[6];
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fanishTime = 0.5f;
    [SerializeField] private GameObject DiceWallParticles;
    [SerializeField] private GameObject boxParticles;
    [SerializeField] private GameObject checkParticles;
    [SerializeField] private AudioClip boxExplode;
    [SerializeField] private GameObject boxShambles;
    [SerializeField] private AudioClip dicemove;
    [SerializeField] private AudioClip diceExplodeSound;
    [SerializeField] private LineRenderer lr;

    [HideInInspector] public bool directionChanged = false;

    public delegate void RollEvent();

    public static RollEvent onRollHitWall;

    private Vector2 direction = Vector2.left;
    private Vector2 nextPoint;

    private int currentNum = 1;

    private bool canRoll = true;

    private bool finalInvoke = false;

    private void Awake()
    {
        StartCoroutine(waitForNextRoll());
    }

    public void resetCurrentNum()
    {
        currentNum = 1;
        spr.sprite = sprites[currentNum-1];
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
        nextPoint = (Vector2)transform.position + dir;
    }
   
    void Update()
    {
        if (canRoll && canMove())
        {
            transform.position = nextPoint;
            AudioManager.instance.playSound(dicemove);
            
            currentNum++;

            if (directionChanged)
            {
                resetCurrentNum();
                directionChanged = false;
            }

            spr.sprite = sprites[currentNum - 1];
            StartCoroutine(waitForNextRoll());
        }

        else if (!canMove() && !finalInvoke)
        {
            StopAllCoroutines();
            finalInvoke = true;
            destroyDirections();
        }
    }

    private bool canMove()
    {
        return !Physics2D.Raycast(transform.position, direction, 1f, wall) && currentNum < 6;
    }

    public void incMove()
    {
        currentNum += 1;
    }

    private void destroyDirections()
    {
        AudioManager.instance.playSound(diceExplodeSound);
        if (direction == Vector2.left || direction == Vector2.right) //Shoots Vertical RayCasts if the direction is horizontal
        {
            lr.SetPosition(0, new Vector2(0, -currentNum - 0.5f));
            lr.SetPosition(1, new Vector2(0, currentNum + 0.5f));

            RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, transform.up, currentNum, destroyObjs);
            RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, -transform.up, currentNum, destroyObjs);

            setCasts(hitUp, hitDown);
        }
        else  //Shoots horizontal RayCasts if the direction is Vertical
        {
            lr.SetPosition(0, new Vector2(-currentNum - 0.5f, 0));
            lr.SetPosition(1, new Vector2(currentNum + 0.5f, 0));

            RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, transform.right, currentNum, destroyObjs);
            RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, -transform.right, currentNum, destroyObjs);

            setCasts(hitRight, hitLeft);
        }
        

        StartCoroutine(finishDeath());
    }
    //loops through each array to destroy any objects filtered by layermask
    private void setCasts(RaycastHit2D[] a, RaycastHit2D[] b)
    {
        if (a.Length != 0)
        {
            foreach (RaycastHit2D h in a)
            {
                filteredDestroy(h);
            }
        }
        if (b.Length != 0)
        {
            foreach (RaycastHit2D h in b)
            {
                filteredDestroy(h);
            }

        }

        if (a.Length != 0 || b.Length != 0)
        {
            StartCoroutine(CameraBehaviour.instance.shake(0.2f, 3f));
        }
    }

    //Destroys gameobjects which layer contain the ones outlined and sets their given particle effects
    private void filteredDestroy(RaycastHit2D h)
    {
        switch (h.collider.gameObject.layer)
        {
            case 8:
                Instantiate(boxParticles, h.collider.transform.position, boxParticles.transform.rotation);
                Instantiate(boxShambles, h.collider.transform.position, boxShambles.transform.rotation);
                AudioManager.instance.playSound(boxExplode);
                Destroy(h.collider.gameObject);
                break;
            case 10:
                Instantiate(checkParticles, h.point, checkParticles.transform.rotation);
                LevelManager.instance.Fail();
                Destroy(h.collider.gameObject);
                break;
        }

    }

    private IEnumerator waitForNextRoll()
    {

        canRoll = false;

        yield return new WaitForSeconds(0.2f);
        nextPoint = (Vector2)transform.position + direction;
        canRoll = true;
    }

    private IEnumerator finishDeath()
    {
        yield return new WaitForSeconds(fanishTime);
        lr.SetPosition(0,Vector2.zero);
        lr.SetPosition(1,Vector2.zero);
        onRollHitWall.Invoke();
        Destroy(gameObject);
    }
}
