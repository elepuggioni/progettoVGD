using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcChoice : MonoBehaviour
{

    public float distance;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject subText;
    public GameObject subBox;

    public Dialogue dialogue;
    private Queue<string> sentences = new Queue<string>();
    private bool isTalking;

    // Update is called once per frame
    void Update()
    {
        distance = DistanceFromObject.DistanceFromTarget;
    }

    void OnMouseOver()
    {
        if (distance <= 3)
        {
            ActionDisplay.SetActive(true);
            ActionText.GetComponent<Text>().text = "Talk";
            ActionText.SetActive(true);
        }
        else
        {
            ActionDisplay.SetActive(false);
            ActionText.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (distance <= 3)
            {
                subBox.SetActive(true);
                subText.SetActive(true);

                sentences.Clear();
                foreach (string sent in dialogue.sentences)
                {
                    sentences.Enqueue(sent);

                }
                DisplayNextSentence();

                ActionDisplay.SetActive(false);
                ActionText.SetActive(false);
                StartCoroutine(ResetChat());
            }
        }

    }

    private void OnMouseExit()
    {
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);
    }

    IEnumerator ResetChat()
    {
        yield return new WaitForSeconds(2.5f);
        subBox.SetActive(false);
        subText.GetComponent<Text>().text = "";
    }

    public void DisplayNextSentence()
    {
        isTalking = true;
        if (sentences.Count == 0)
        {
            StartCoroutine(ResetChat());
            return;
        }
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine (Aspetta(sentence));

    }

    IEnumerator Aspetta(string frase)
    {

        if (isTalking) subText.GetComponent<Text>().text = frase;

        if (Input.GetKeyUp(KeyCode.E))
        {
            isTalking = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && isTalking)
        {
            DisplayNextSentence();
            isTalking = false;
            yield return null;
        }

    }
}


/* subText.GetComponent<Text>().text = frase;
yield return new WaitForSeconds(2.5f);
if (Input.GetKey(KeyCode.E)) DisplayNextSentence(); */