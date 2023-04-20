using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource ad;

    private void Awake() => instance = this;

    public void playSound(AudioClip clip)
    {
        ad.PlayOneShot(clip);
    }
}
