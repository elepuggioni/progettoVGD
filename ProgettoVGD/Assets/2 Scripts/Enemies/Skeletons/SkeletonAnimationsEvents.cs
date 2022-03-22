using UnityEngine;

/* In questo script sono implementate le funzioni che si
 * egeguono negli eventi delle animazioni degli scheletri */
public class SkeletonAnimationsEvents : MonoBehaviour
{
   //Riferimenti ad altri script
   private SkeletonAttackColliders skeletonAttackColliders;
   private EnemyController enemyController;

   private void Awake()
   {
      skeletonAttackColliders = GetComponentInChildren<SkeletonAttackColliders>();
      enemyController = GetComponentInParent<EnemyController>();
   }

   // Attiva i collider dell'attacco dello scheletro
   public void EnableColliders()
   {
      skeletonAttackColliders.EnableLegColliders();
   }

   // Disattiva i collider dell'attacco dello scheletro
   public void DisableColliders()
   {
      skeletonAttackColliders.DisableLegColliders();
      //Aggiorna il booleano che evita che con un attacco vengano attivate più collisioni
      enemyController.PlayerHitted = false;
   }
}
