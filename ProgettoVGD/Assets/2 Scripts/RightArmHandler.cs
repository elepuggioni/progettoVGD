using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RightArmHandler : MonoBehaviour
{
    public Collider rightArmCollider;
    public Collider rightForeArmCollider;
    public Collider swordCollider;

    private void Awake()
    {
        rightArmCollider = transform.Find("RightArmCollider").GetComponent<Collider>();
        rightArmCollider.gameObject.SetActive(true);
        rightArmCollider.isTrigger = true;
        rightArmCollider.enabled = false;

        rightForeArmCollider = transform.Find("RightForeArm").Find("RightForeArmCollider").GetComponentInChildren<Collider>();
        rightForeArmCollider.gameObject.SetActive(true);
        rightForeArmCollider.isTrigger = true;
        rightForeArmCollider.enabled = false;
        
        swordCollider = transform.Find("RightForeArm").Find("RightHand").GetComponentInChildren<Collider>();
        swordCollider.gameObject.SetActive(true);
        swordCollider.isTrigger = true;
        swordCollider.enabled = false;
    }

    public void EnableAttackCollider()
    {
        rightArmCollider.enabled = true;
        rightForeArmCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        swordCollider.enabled = false;
        rightArmCollider.enabled = false;
        rightForeArmCollider.enabled = false;
    }

    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }
}
