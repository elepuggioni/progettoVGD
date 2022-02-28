using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraThirdPerson : MonoBehaviour
{
    public bool lockCursore; //bloccare il cursore
    public float mouseSensitivity = 10; 
    public Transform obiettivo;     // obiettivo che la camera deve puntare
    private float dstFromTarget = 3.5f; // distanza dall'obiettivo
    public Vector2 pitchMinMax = new Vector2(-40, 85); //valore minimo e massimo per il beccheggio

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw; //imbardata (asse verticale)
    float pitch; //beccheggio (asse trasversale)

    void Start()
    {
        if (lockCursore)
        {
            Cursor.lockState = CursorLockMode.Locked; //cursore bloccato al centro dello schermo
            Cursor.visible = false; // cursore invisibile
        }
    }

    void LateUpdate()
    {
        
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity; // aumento il valore dell'imbardata 
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity; // decremento il valore del beccheggio
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y); //blocco il valore del beccheggio tra il minimo e il massimo

            // Muove la camera verso la posizione dell'obiettivo
            currentRotation = Vector3.SmoothDamp(currentRotation,
                                                new Vector3(pitch, yaw),
                                                ref rotationSmoothVelocity,
                                                rotationSmoothTime);

            // Applica una rotazione 3D con gli angoli di Eulero
            transform.eulerAngles = currentRotation;
            
            // Cambia la posizione del transform
            transform.position = obiettivo.position - transform.forward * dstFromTarget;
        }
    

}
