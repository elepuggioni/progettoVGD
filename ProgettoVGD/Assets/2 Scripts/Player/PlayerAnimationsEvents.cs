using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* In questo script sono implementate le funzioni che si
 * egeguono negli eventi delle animazioni del player principale */
public class PlayerAnimationsEvents : MonoBehaviour
{
    //Riferimenti ad altri componenti del player
    private PlayerController playerController;
    private Animator animator;
    private PlayerAttackColliders playerAttackColliders;
    private AudioHandler audioHandler;

    [Header("Sounds")]
    [SerializeField] [Tooltip("Clip Audio per i passi del Player")]
    private AudioSource Footstep;
    [SerializeField] [Tooltip("Clip Audio per il roll del Player")]
    private AudioSource RollSound;
    [SerializeField] [Tooltip("Clip Audio per l'attacco del Player")]
    private AudioSource AttackSound;
    [SerializeField] [Tooltip("Clip Audio per la morte del Player")]
    private AudioSource DeathSound;

    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        playerAttackColliders = GetComponentInChildren<PlayerAttackColliders>();
        audioHandler = FindObjectOfType<AudioHandler>();
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

    public void PlayAttackSound()
    {
        AttackSound.Play();
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
    
    public void PlayFootStep()
    {
        if(!playerController.isAttacking && !playerController.isInteracting) 
            Footstep.Play();
    }

    public void PlayRollSound()
    {
        RollSound.Play();
    }

    public void PlayDeathSound()
    {
        //Stoppa tutte le musiche di background
        audioHandler.StandardBackground.Stop();
        audioHandler.SecondaryBossBackground.Stop();
        audioHandler.BossBackground.Stop();
        
        DeathSound.Play();
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5.0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

}
