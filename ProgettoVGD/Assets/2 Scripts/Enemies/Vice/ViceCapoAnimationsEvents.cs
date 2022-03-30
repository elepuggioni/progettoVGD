using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViceCapoAnimationsEvents : MonoBehaviour
{
    private Animator animator;
    private ViceCapoAttackColliders viceCapoAttackColliders;
    private ViceCapoController viceCapoController;

    [Header("Sounds")] 
    [SerializeField]
    private AudioSource ViceCapoFootstep;
    [SerializeField]
    private AudioSource ViceCapoAttack;

    private void Awake()
    {
        viceCapoAttackColliders = GetComponentInChildren<ViceCapoAttackColliders>();
        animator = GetComponent<Animator>();
        viceCapoController = GetComponent<ViceCapoController>();
    }

    public void PlayFootstepVice()
    {
        ViceCapoFootstep.Play();
    }

    public void PlayAttackSound()
    {
        ViceCapoAttack.Play();
    }

    public void EnableAttackColliders()
    {
        viceCapoAttackColliders.EnableColliders();
    }

    public void DisableAttackColliders()
    {
        viceCapoAttackColliders.DisableColliders();
        viceCapoController.playerHitted = false;
    }
}
