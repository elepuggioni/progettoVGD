using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public bool dialogueStarted;

    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueBox;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        
    }
    public void StartDialogue(Dialogue dialogue){
        dialogueStarted = true;
        dialogueBox.SetActive(true);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if(sentences.Count == 0 ){
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //se si avanza il testo prima che finisca l'animazione si interrompe l'animazione
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            //aspetta 1 frame
            yield return null;
        }

    }

    public void EndDialogue(){
        dialogueBox.SetActive(false);
        dialogueStarted = false;
    }
}
