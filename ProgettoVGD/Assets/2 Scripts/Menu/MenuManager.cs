
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject buttons; // riferimento ai pulsanti del menu inziale
    [SerializeField] GameObject comands; // riferimento ai comandi scritti 
    [SerializeField] GameObject backButton; // riferimento al pulsante 'Indietro'
    [SerializeField] GameObject rules; // riferiemento alle regole scritte
    [SerializeField] GameObject textBox; // riferimento alla textBox
    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private AudioSource NewGameSound;


    private void Start()
    {
        BackgroundMusic.PlayDelayed(0.8f);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Villaggio"); // Avvia il gioco
        NewGameSound.Play();
    }

    public void Regole()
    {
        buttons.SetActive(false); // Disattiva i pulsanti 
        textBox.SetActive(true); // Mostra la textBox
        rules.SetActive(true); // Mostra le regole
        backButton.SetActive(true); // Mostra il pulsante indietro
    }

    public void Comandi()
    {
        buttons.SetActive(false);
        backButton.SetActive(true);
        comands.SetActive(true); // Mostra i comandi
    }

    public void Back()
    {
        textBox.SetActive(false);
        rules.SetActive(false); 
        buttons.SetActive(true);
        backButton.SetActive(false);
        comands.SetActive(false); 
    }
}

