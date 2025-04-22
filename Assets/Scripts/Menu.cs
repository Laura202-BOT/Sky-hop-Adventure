#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void OnPlayButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScenes");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}


#endif