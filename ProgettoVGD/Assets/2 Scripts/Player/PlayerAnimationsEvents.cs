using System;
using UnityEngine;

/* In questo script sono implementate le funzioni che si
 * egeguono negli eventi delle animazioni del player principale */
public class PlayerAnimationsEvents : MonoBehaviour
{
    //Riferimenti ad altri componenti del player
    private PlayerController playerController;
    private Animator animator;
    private PlayerAttackColliders playerAttackColliders;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        playerAttackColliders = GetComponentInChildren<PlayerAttackColliders>();
    }

    // Permette di attivare un animazione precisa
    public void PlayTargetAnimation(String targetAnimation)
    {
        animator.CrossFade(targetAnimation, 0.2f);
    }
    
    // Eventi dell'animazione di attacco
    #region Attack animations Events
    
    // Aggiorna la variabile booleana del playerController e attiva i box collider del braccio
    public void startAttack()
    {
        playerController.isInteracting = true;
        playerAttackColliders.EnableAttackCollider();
    }
    
    // Attiva il box collider della spada
    public void EnableSword()
    {
        playerAttackColliders.EnableSwordCollider();
    }

    // Aggiorna il boolean del PlayerController e resetta il boolean di controllo dei nemici
    public void stopAttack()
    {
        playerController.isAttacking = false;
        ResetHit();
    }

    // Aggiorna il boolean del PlayerController e disattiva i box collider dell'attacco
    public void StopIsInteracting()
    {
        playerController.isInteracting = false;
        playerAttackColliders.DisableAttackCollider();
    }

    // Resetta il boolean di controllo di ogni EnemyController nella scena
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
