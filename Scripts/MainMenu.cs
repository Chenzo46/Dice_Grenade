using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void playSound(AudioClip clip)
    {
        AudioManager.instance.playSound(clip);
    }

}
