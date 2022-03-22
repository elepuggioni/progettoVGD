using UnityEngine;

// Gestisce i trigger dei box collider
public class TriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Esegue questo blocco if quando è il player ad aver eseguito un attacco
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
        
            //Esegue solo se il GameObject "other" possiede un Enemycontroller e se non sia gia stato colpito
            if (enemyController != null && !enemyController.AlreadyHitted)
            {
                //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                enemyController.AlreadyHitted = true;
                enemyController.TakeDamage(1);
            }
        } 
        
        //Esegue questo blocco if quando è un nemico ad aver eseguito un attacco verso il player
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            EnemyController enemyController = GetComponentInParent<EnemyController>();
            
            //Esegue solo se il player non sia gia stato colpito
            if (playerController != null && enemyController != null && !enemyController.PlayerHitted)
            {
                //Specifica che "other" è stato colpito per evitare collisioni multiple in un solo attacco
                enemyController.PlayerHitted = true;
                playerController.TakeDamage(1);
            }
        }
    }
}