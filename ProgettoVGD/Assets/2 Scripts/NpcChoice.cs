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


    // Update is called once per frame
    void Update()
    {
        distance = DistanceFromObject.DistanceFromTarget;
    }

    void OnMouseOver()
    {
        if(distance <= 3)
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
            if(distance <= 3)
            {
                subBox.SetActive(true);
                subText.GetComponent<Text>().text = "Prova 1 2 3";
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
}
