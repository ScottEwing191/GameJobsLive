using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Volume : MonoBehaviour
{
    private static Volume instance;
    public int order = 0;
    public bool isVolumeOn = true;
    [SerializeField] private GameObject mainCamera;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Volume[] volumes = FindObjectsOfType<Volume>();
        foreach (var v in volumes) {            // stops two volume game objects fromexisting when the player goes back to the main menu.
            if (v.order == 1) {                 // the older Gameobject is deleted
                isVolumeOn = v.isVolumeOn;
                Destroy(v.gameObject);
            }
        }
        if (!isVolumeOn) {
            Camera.main.GetComponent<AudioListener>().enabled = false;
        }
        print("Hellow");
        order++;
    }
    public void VolumeOn() {
        //AudioListener listen =  Camera.main.GetComponent<AudioListener>();
        //AudioListener listen = mainCamera.GetComponent<AudioListener>();
        //mainCamera.SetActive(true);
        AudioListener.volume = 1;
        //listen.enabled = true;
        isVolumeOn = true;
    }

    public void VolumeOff() {
        //Camera.main.GetComponent<AudioListener>().enabled = false;
        //mainCamera.SetActive(false);


        //AudioListener listen = mainCamera.GetComponent<AudioListener>();
        //listen.enabled = false;
        //listen.volume
        AudioListener.volume = 0;
        isVolumeOn = false;
    }
}
