using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraThirdPerson : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //cursore bloccato al centro dello schermo
        Cursor.visible = false; // cursore invisibile
    }

}
