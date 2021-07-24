using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    
    // Normally this is done in the Gamemanager script. But since the Level scene doesn't have A Game Manager It's getting done here
    void Start()
    {
        Volume gameVolume = FindObjectOfType<Volume>();

        if (gameVolume != null) {
            if (!gameVolume.isVolumeOn) {
                //Camera.main.GetComponent<AudioListener>().enabled = false;
                AudioListener.volume = 0;
            }
        }
    }

   
}
