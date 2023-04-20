using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    private Vector2 direction;

    private void Awake()
    {
        direction = transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("dice"))
        {
            DiceRoller dc = collision.GetComponent<DiceRoller>();

            dc.directionChanged = true;

            dc.setDirection(direction);
        }
    }
}
