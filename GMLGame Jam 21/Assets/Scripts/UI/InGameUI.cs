using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject panelDisable;
    public GameObject panelEnable;
    public string sceneName;

    public void OpenPanel()
    {
        panelEnable.SetActive(true);
    }

    public void ClosePanel()
    {
        panelDisable.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}
