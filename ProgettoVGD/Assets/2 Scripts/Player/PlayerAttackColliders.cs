using UnityEngine;

// Gestisce i collider per la rilevazione di collisioni durante gli attacchi del player
public class PlayerAttackColliders : MonoBehaviour
{
    //Riferimenti ai collider per l'attacco
    public Collider rightArmCollider;
    public Collider rightForeArmCollider;
    public Collider swordCollider;

    private void Awake()
    {
        rightArmCollider = transform.Find("RightArmCollider").GetComponent<Collider>();
        rightArmCollider.gameObject.SetActive(true);
        rightArmCollider.isTrigger = true;
        rightArmCollider.enabled = false;

        rightForeArmCollider = transform.Find("RightForeArm").Find("RightForeArmCollider").GetComponent<Collider>();
        rightForeArmCollider.gameObject.SetActive(true);
        rightForeArmCollider.isTrigger = true;
        rightForeArmCollider.enabled = false;
        
        swordCollider = transform.Find("RightForeArm").Find("RightHand").GetComponentInChildren<Collider>();
        swordCollider.gameObject.SetActive(true);
        swordCollider.isTrigger = true;
        swordCollider.enabled = false;
    }

    // Abilita i collider del braccio del player
    public void EnableAttackCollider()
    {
        rightArmCollider.enabled = true;
        rightForeArmCollider.enabled = true;
    }

    // Abilita il collider della spada del player
    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }
    
    // Disabilita tutti i collider dell'attacco del player
    public void DisableAttackCollider()
    {
        swordCollider.enabled = false;
        rightArmCollider.enabled = false;
        rightForeArmCollider.enabled = false;
    }
}
