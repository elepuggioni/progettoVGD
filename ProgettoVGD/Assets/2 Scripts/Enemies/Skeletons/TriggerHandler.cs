using System;
using UnityEngine;

// Gestisce i trigger dei box collider
public class TriggerHandler : MonoBehaviour
{
    private PlayerController playerController;
    private ViceCapoController viceCapoController;
    private BossController bossController;
    private EnemyController enemyController;
    private DialogueManager dialogueManager;
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Boss":
                bossController = other.GetComponent<BossController>();

                if (bossController != null && !bossController.AlreadyHitted)
                {
                    bossController.AlreadyHitted = true;
                    bossController.TakeDamage(1);
                }
                break;
            
            case "ViceCapo":
                viceCapoController = other.GetComponent<ViceCapoController>();
                dialogueManager = FindObjectOfType<DialogueManager>();

                if (dialogueManager.getViceCapo() && viceCapoController != null && !viceCapoController.AlreadyHitted)
                {
                    viceCapoController.AlreadyHitted = true;
                    viceCapoController.TakeDamage(1);
                }
                break;
            
            case "Player":
                playerController = other.GetComponent<PlayerController>();
            
                if (CompareTag("ViceCapo Collider"))
                {
                    viceCapoController = GetComponentInParent<ViceCapoController>();

                    if (playerController != null && viceCapoController != null && !viceCapoController.playerHitted)
                    {
                        viceCapoController.playerHitted = true;
                        playerController.TakeDamage(2);
                    }
                }
                else
                {
                    enemyController = GetComponentInParent<EnemyController>();

                    //Esegue solo se il player non sia gia stato colpito
                    if (playerController != null && enemyController != null && !enemyController.PlayerHitted)
                    {
                        //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                        enemyController.PlayerHitted = true;
                        if (playerController.armaturaAcquisita)
                            playerController.TakeDamage(1);
                        else
                            playerController.TakeDamage(2);
                    }
                }
                break;
            
            case "Enemy":
                enemyController = other.GetComponent<EnemyController>();
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                //Esegue solo se il GameObject "other" possiede un Enemycontroller e se non sia gia stato colpito
                if (enemyController != null && !enemyController.AlreadyHitted)
                {
                    //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                    enemyController.AlreadyHitted = true;
                    if (playerController.spadaAcquisita)
                        enemyController.TakeDamage(2);
                    else
                        enemyController.TakeDamage(1);
                }
                break;
        }

        
        /*
        //Esegue questo blocco if quando è il player ad aver eseguito un attacco
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            //Esegue solo se il GameObject "other" possiede un Enemycontroller e se non sia gia stato colpito
            if (enemyController != null && !enemyController.AlreadyHitted)
            {
                //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                enemyController.AlreadyHitted = true;
                if (playerController.spadaAcquisita)
                    enemyController.TakeDamage(2);
                else
                    enemyController.TakeDamage(1);
            }
        } */
        /*
        //Esegue questo blocco if quando è un nemico ad aver eseguito un attacco verso il player
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            
            if (CompareTag("ViceCapo Collider"))
            {
                ViceCapoController viceCapoController = GetComponentInParent<ViceCapoController>();

                if (playerController != null && viceCapoController != null && !viceCapoController.playerHitted)
                {
                    viceCapoController.playerHitted = true;
                    playerController.TakeDamage(2);
                }
            }
            else
            {
                EnemyController enemyController = GetComponentInParent<EnemyController>();

                //Esegue solo se il player non sia gia stato colpito
                if (playerController != null && enemyController != null && !enemyController.PlayerHitted)
                {
                    //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                    enemyController.PlayerHitted = true;
                    if (playerController.armaturaAcquisita)
                        playerController.TakeDamage(1);
                    else
                        playerController.TakeDamage(2);
                }
            }
        }*/
    }
}