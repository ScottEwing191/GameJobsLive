using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
