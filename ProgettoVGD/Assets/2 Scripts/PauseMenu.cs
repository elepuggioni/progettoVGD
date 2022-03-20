using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private PlayerController player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();   
    }

    public void Pause() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        player.isInTheMenu = true; // Segna che il player è in pause
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freezza il gioco
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player.isInTheMenu = false; // Segna che il player non è piu in pausa
        pauseMenu.SetActive(false);
        
        Time.timeScale = 1f; // il gioco torna alla velocità normale
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

}
