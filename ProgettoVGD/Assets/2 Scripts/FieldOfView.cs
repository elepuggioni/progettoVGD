using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controlla se un ogject target è presente nel campo
 * visivo dell'object a cui è assegnato questro script
 */
public class FieldOfView : MonoBehaviour
{
    [Tooltip("Grandezza del raggio della sfera che indica il campo visivo")]
    public float radius;
    
    [Tooltip("Ampiezza dell'angolo del campo visivo effettivo")]
    [Range(0,360)]
    public float angle;
    
    [SerializeField]
    [Tooltip("Layer mask che indica il layer del target da cercare nel campo visivo")]
    private LayerMask targetMask;
    
    [Tooltip("Boolean che indica se il target è nel campo visivo")]
    public bool isVisible;
    
    [Tooltip("Transform del object target")]
    public Transform targetTransform;
    
    private void Start()
    {
        //Avvia la coroutine che effettua la ricerca
        StartCoroutine(FieldOfViewRoutine());
    }

    // Coroutine che cerca il target nel campo visivo
    private IEnumerator FieldOfViewRoutine()
    {
        //Intervallo di ricerca per evitare di fare ricerce ridondanti
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    // Procedura che effettua la ricerca del target
    private void FieldOfViewCheck()
    {
        //Array di collider generico in cui salva i collider degli object corrispondenti alla ricerca nel campo visivo
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //Se l'array precedentemente dichiarato non è vuoto
        if (rangeChecks.Length != 0)
        {
            //Riferimento al transform dell'object trovato nel campo visivo
            Transform target = rangeChecks[0].transform;
            
            //Direzione in cui si trova il target rispetto al object che lo ha nel campo visivo
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            /* Se l'angolo tra l'asse in cui è indirizzato il campo visivo e la
             * direzione del target è compreso nel campo visivo effettivo*/ 
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                //Calcola la distanza tra questo object e il target
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //Se il target è realmente nel campo visivo dell'object setta il boolean a true, altrimento a false
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget))
                {
                    targetTransform = target;
                    isVisible = true;
                }
                else
                    isVisible = false; 
            }
            //Se l'angolo invece è tale che il target è fuori dal campo visivo reale setta il boolean a false
            else
                isVisible = false;
        }
        //Se il boolean è rimasto a true da un controllo precedente e il target è fuori dal campo visivo, lo setta a false
        else if (isVisible)
            isVisible = false;
    }
}