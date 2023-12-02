using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionMenu : MonoBehaviour
{
    public void Pause(GameObject go)
    {
        go.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(GameObject go)
    {
        go.SetActive(false);
        Time.timeScale = 1;
    }
}
