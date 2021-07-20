using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
    }

}
