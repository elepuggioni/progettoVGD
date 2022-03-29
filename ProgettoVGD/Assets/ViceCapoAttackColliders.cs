using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViceCapoAttackColliders : MonoBehaviour
{
    public BoxCollider ArmCollider;
    public BoxCollider ForeArmCollider;
    public BoxCollider SwordCollider;

    private void Awake()
    {
        ArmCollider.isTrigger = true;
        ArmCollider.enabled = false;

        ForeArmCollider.isTrigger = true;
        ForeArmCollider.enabled = false;

        SwordCollider.isTrigger = true;
        SwordCollider.enabled = false;
    }

    public void EnableColliders()
    {
        ArmCollider.enabled = true;
        ForeArmCollider.enabled = true;
        SwordCollider.enabled = true;
    }
    
    public void DisableColliders()
    {
        ArmCollider.enabled = false;
        ForeArmCollider.enabled = false;
        SwordCollider.enabled = false;
    }
}
