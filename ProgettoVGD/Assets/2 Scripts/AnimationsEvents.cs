using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private RightArmHandler rightArmHandler;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rightArmHandler = GetComponentInChildren<RightArmHandler>();
    }

    
    public void PlayTargetAnimation(String targetAnimation)
    {
        animator.CrossFade(targetAnimation, 0.2f);
    }
    
    #region Attack animations Events
    public void startAttack()
    {
        playerController.isInteracting = true;
        rightArmHandler.EnableAttackCollider();
    }
    
    public void EnableSword()
    {
        rightArmHandler.EnableSwordCollider();
    }

    public void stopAttack()
    {
        playerController.isAttacking = false;
        ResetHit();
    }

    public void StopIsInteracting()
    {
        playerController.isInteracting = false;
        rightArmHandler.DisableAttackCollider();
    }

    private void ResetHit()
    {
        EnemyController[] enemyControllers = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemyController in enemyControllers)
        {
            enemyController.AlreadyHitted = false;

        }
    }
    #endregion

}
