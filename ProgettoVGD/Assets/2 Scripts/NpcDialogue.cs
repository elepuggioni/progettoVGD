using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcDialogue : MonoBehaviour
{

    public float distance;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject subText;
    public GameObject subBox;
    private Quaternion rotazioneNPC;

    public GameObject thePlayer;
    private Animator animator;

    [SerializeField]
    private Dialogue dialogue;
    private FieldOfView fov;
    private Queue<string> sentences = new Queue<string>();


    private void Start()
    {
        rotazioneNPC = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
        animator = GetComponent<Animator>();
        fov = thePlayer.GetComponentInChildren<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = DistanceFromObject.DistanceFromTarget;
        
        /*
        if (fov.isVisible)
        {
            ActionDisplay.SetActive(true);
            ActionText.GetComponent<Text>().text = "Parla [E]";
            ActionText.SetActive(true);
        }
        else
        {
            ActionDisplay.SetActive(false);
            ActionText.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E))
        {
            thePlayer.GetComponent<PlayerController>().enabled = false;

            this.transform.LookAt(new Vector3(thePlayer.transform.position.x, this.transform.position.y,
                thePlayer.transform.position.z));
            subBox.SetActive(true);
            subText.SetActive(true);

            if (this.tag == "Cognata")
                animator.SetBool("Stendere", false);

            sentences.Clear();
            foreach (string sent in dialogue.sentences)
            {
                sentences.Enqueue(sent);

            }

            DisplayNextSentence();
            ActionDisplay.SetActive(false);
            ActionText.SetActive(false);
        }
        */
    }

    
    void OnMouseOver()
    {
        if (distance <= 3 )
        {
            ActionDisplay.SetActive(true);
            ActionText.GetComponent<Text>().text = "Parla [E]";
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
                thePlayer.GetComponent<PlayerController>().enabled = false;

                this.transform.LookAt(new Vector3(thePlayer.transform.position.x, this.transform.position.y, thePlayer.transform.position.z));
                subBox.SetActive(true);
                subText.SetActive(true);

                if (this.tag == "Cognata")
                    animator.SetBool("Stendere", false);

                sentences.Clear();
                foreach (string sent in dialogue.sentences)
                {
                    sentences.Enqueue(sent);

                }

                DisplayNextSentence();
                ActionDisplay.SetActive(false);
                ActionText.SetActive(false);  
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
        yield return new WaitForSeconds(1.5f);
        thePlayer.GetComponent<PlayerController>().enabled = true;

        if(this.tag == "Cognata")
            animator.SetBool("Stendere", true);

        this.transform.rotation = rotazioneNPC;
        subBox.SetActive(false);
        subText.GetComponent<Text>().text = "";
        subText.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(ResetChat());
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine (WaitAndRead(sentence));

    }

    IEnumerator WaitAndRead(string frase)
    {
        subText.GetComponent<Text>().text = frase;
        yield return new WaitForSeconds(2.5f);
        DisplayNextSentence(); 
    }

}


