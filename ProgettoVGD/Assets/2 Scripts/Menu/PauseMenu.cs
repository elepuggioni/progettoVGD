using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // riferimento al menu di pausa
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject loadButton;
    private PlayerController player; // riferimento al player
    private AudioHandler audioHandler;

    private void Start()
    {
        // Prendi il player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

    public void Pause() 
    {
        Cursor.lockState = CursorLockMode.Confined; // Confina il cursore e lo rende visibile
        Cursor.visible = true;
        player.isInTheMenu = true; // Segna che il player è in pause
        if (!player.bossBattleIsStarted) // Se la boss battle non è inziata puoi ancora salvare e caricare
        {
            pauseMenu.SetActive(true); // Attiva il menu di pausa
            saveButton.SetActive(true);
            loadButton.SetActive(true);
        }
        else if (player.bossBattleIsStarted) // se la boss battle è inziata non puoi piu salvare o caricare
        {
            pauseMenu.SetActive(true);
            saveButton.SetActive(false);
            loadButton.SetActive(false);
        }

        Time.timeScale = 0f; // Freezza il gioco
    }

    public void Resume()
    {
        Cursor.visible = false; // Blocca e rende invisibile il cursore
        Cursor.lockState = CursorLockMode.Locked;

        player.isInTheMenu = false; // Segna che il player non è piu in pausa
        pauseMenu.SetActive(false); // Disattiva il menu di pausa
        
        Time.timeScale = 1f; // il gioco torna alla velocità normale
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneID); // Carica la scena avente come id il valore di sceneid
    }

}
