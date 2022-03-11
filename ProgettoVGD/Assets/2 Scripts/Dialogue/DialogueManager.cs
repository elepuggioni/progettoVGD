using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public bool dialogueStarted;
    
    public GameObject subText;
    public GameObject subBox;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    private Quaternion rotazioneNPC;
    private Animator _animator;
    private GameObject _npc;
    

    public GameObject _player;
    

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue, GameObject npc){
        dialogueStarted = true;
        
        _player.GetComponent<PlayerController>().enabled = false;

        _npc = npc;
        rotazioneNPC = npc.transform.rotation;

        if(!(_npc.CompareTag("Prete")))
            _npc.transform.LookAt(_player.transform.position);

        _animator = npc.GetComponent<Animator>();
        
        if (_npc.CompareTag("Cognata"))
            _animator.SetBool("Stendere", false);
        
        subBox.SetActive(true);
        subText.SetActive(true);
        
        sentences.Clear();
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false); 
    }
    
    public void DisplayNextSentence(){
        if(sentences.Count == 0 )
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //se si avanza il testo prima che finisca l'animazione si interrompe l'animazione
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence){
        subText.GetComponent<Text>().text = "";
        foreach(char letter in sentence){
            subText.GetComponent<Text>().text += letter;
            //aspetta 1 frame
            yield return null;
            if (Input.anyKey)
            {
                subText.GetComponent<Text>().text = sentence;
                break;
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.anyKey); //.GetKeyDown(KeyCode.E)); // new WaitForSeconds(2.5f);
        DisplayNextSentence();
    }
    
    public void EndDialogue(){
        subBox.SetActive(false);
        subText.GetComponent<Text>().text = "";
        subText.SetActive(false);
        dialogueStarted = false;
        _player.GetComponent<PlayerController>().enabled = true;
        _npc.transform.rotation = rotazioneNPC;
        if (_npc.CompareTag("Cognata"))
            _animator.SetBool("Stendere", true);
    }
}
