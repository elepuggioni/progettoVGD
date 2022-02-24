using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {

        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Villaggio");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
    }
}

