using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            LevelManager.instance.incChecks();
            AudioManager.instance.playSound(collectSound);
            Destroy(gameObject);
        }
    }
}
