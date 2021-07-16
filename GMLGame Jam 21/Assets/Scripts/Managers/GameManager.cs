using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    /*protected override void Awake() { 
        base.Awake();
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ResetLevel() {

    }

    public void LevelComplete() {
        print("LEVEL COMPLETE");
        // Load the next scene if it exists otherwise load the main menu
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.sceneCountInBuildSettings;
        if (currentBuildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(currentBuildIndex + 1);
        }
        else {
            SceneManager.LoadScene(0);
        }
    }
}
