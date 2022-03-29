using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource StandardBackground;
    public AudioSource SecondaryBossBackground;
    public AudioSource BossBackground;
    public AudioSource SkeletonHitted;
    public AudioSource EnemyHitted;
    public AudioSource EnemyKilled;


    // Start is called before the first frame update
    void Start()
    {
        StandardBackground.Play();
    }
    
}
