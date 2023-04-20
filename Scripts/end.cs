using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && LevelManager.instance.allChecksCollected())
        {
            LevelManager.instance.advance();
        }
    }
}
