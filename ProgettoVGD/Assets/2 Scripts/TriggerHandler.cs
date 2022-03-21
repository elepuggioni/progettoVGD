using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    private RightArmHandler rightArmHandler;

    private void Awake()
    {
        rightArmHandler = FindObjectOfType<RightArmHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
        
            if (enemyController != null && !enemyController.AlreadyHitted)
            {
                enemyController.AlreadyHitted = true;
                enemyController.TakeDamage(1);
            }
        }
    }
}