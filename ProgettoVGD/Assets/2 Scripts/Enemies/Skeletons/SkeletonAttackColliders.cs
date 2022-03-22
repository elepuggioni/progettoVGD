using UnityEngine;

// Gestisce i collider per la rilevazione di collisioni durante gli attacchi degli scheletri
public class SkeletonAttackColliders : MonoBehaviour
{
    //Riferimenti ai collider per l'attacco
    public Collider upLegCollider;
    public Collider legCollider;
    public Collider footCollider;

    private void Awake()
    {
        upLegCollider = transform.Find("UpLegCollider").GetComponent<Collider>();
        upLegCollider.isTrigger = true;
        upLegCollider.enabled = false;

        legCollider = transform.Find("PT_Medieval_Male_Armor_RightLeg").Find("LegCollider").GetComponent<Collider>();
        legCollider.isTrigger = true;
        legCollider.enabled = false;
        
        footCollider = transform.Find("PT_Medieval_Male_Armor_RightLeg").Find("PT_Medieval_Male_Armor_RightFoot").GetComponentInChildren<Collider>();
        footCollider.isTrigger = true;
        footCollider.enabled = false;
    }

    // Abilita i box collider
    public void EnableLegColliders()
    {
        upLegCollider.enabled = true;
        legCollider.enabled = true;
        footCollider.enabled = true;
    }
    
    // Disabilita i box collider
    public void DisableLegColliders()
    {
        upLegCollider.enabled = false;
        legCollider.enabled = false;
        footCollider.enabled = false;
    }
}
