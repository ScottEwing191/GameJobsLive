using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    CharacterController2D characterController2D;
    HitchedRope[] hitchedRopeArray;
    /*protected override void Awake() { 
        base.Awake();
    }*/

    private void Start() {
        //hitchedRopeArray = FindObjectsOfType(typeof(HitchedRope)) as HitchedRope;
        hitchedRopeArray = FindObjectsOfType<HitchedRope>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController2D = GameObject.Find("Player").GetComponent<CharacterController2D>();
    }


    public void ResetLevel() {
        characterController2D.ResetPlayer();
        UIManager.Instance.ResetUI();
        ResetHitchedRopes();
    }

    private void ResetHitchedRopes() {
        foreach (var rope in hitchedRopeArray) {
            rope.ResetHitch();
        }
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
