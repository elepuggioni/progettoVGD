using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource StandardBackground;
    public AudioSource SecondaryBossBackground;
    public AudioSource BossBackground;


    // Start is called before the first frame update
    void Start()
    {
        StandardBackground.Play();
    }
    
}
