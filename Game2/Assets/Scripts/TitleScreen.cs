using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }
    public void Quit()
    {
        Application.Quit();
    }
}
