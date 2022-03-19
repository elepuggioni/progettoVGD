using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public float x,y,z;
    public PlayerController pc;
    public CharacterController cc;
    public PauseMenu pm;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;



        PlayerPrefs.SetFloat("x", x);
        PlayerPrefs.SetFloat("y", y);
        PlayerPrefs.SetFloat("z", z);
    }

    public void Load(){
        x = PlayerPrefs.GetFloat("x");
        y = PlayerPrefs.GetFloat("y");
        z = PlayerPrefs.GetFloat("z");

        pc.enabled = false;
        cc.enabled = false;
        transform.position = new Vector3(x,y,z);
        pc.enabled = true;
        cc.enabled = true;
        
        pm.Resume();

    }
}
