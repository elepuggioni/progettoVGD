using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject comands;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject rules;
    [SerializeField] GameObject textBox;

    public void NewGame()
    {
        SceneManager.LoadScene("Villaggio");
    }

    public void Regole()
    {
        buttons.SetActive(false);
        textBox.SetActive(true);
        rules.SetActive(true);
        backButton.SetActive(true);
    }

    public void Comandi()
    {
        buttons.SetActive(false);
        backButton.SetActive(true);
        comands.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}

