using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void GoToStage1()
    {
        SceneManager.LoadScene("Stage1");
    }
    public void GoToStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit() // 어플리케이션 종료
        #endif
    }
}
