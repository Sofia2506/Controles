using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float RotationSpeed = 1;
    public Transform Target;
    public Transform Jugador;

    float mouseX;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate(){
        CamControl();
    }

    void CamControl(){
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        transform.LookAt(Target);
        Target.rotation = Quaternion.Euler(0, mouseX, 0); 
        Jugador.rotation = Quaternion.Euler(0, mouseX, 0); 
    }
}
