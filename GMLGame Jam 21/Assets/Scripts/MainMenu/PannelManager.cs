using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelManager : MonoBehaviour
{
    public GameObject panelDisable;
    public GameObject panelEnable;

    public void OpenPanel()
    {
        panelDisable.SetActive(false);
        panelEnable.SetActive(true);
    }
}
