using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float SensitivityX;
    public float SensitivityY;

    public Transform Orientation;

    float RotationX;
    float RotationY;

    public bool AllowPlayerCamMovement;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //[DELETE] USED FOR UNITY EDITOR DELETE BEFORE BUILD
        if (Input.GetKeyDown(KeyCode.L))
        { 
            AllowPlayerCamMovement = !AllowPlayerCamMovement;
        
        }


        if (AllowPlayerCamMovement == true)  
        {
            // Get mouse input
            float MouseX = Input.GetAxisRaw("Mouse X") * SensitivityX;
            float MouseY = Input.GetAxisRaw("Mouse Y") * SensitivityY;

            RotationY += MouseX;

            RotationX -= MouseY;
            RotationX = Mathf.Clamp(RotationX, -90f, 90f);

            // Rotate camera and Orientation
            transform.rotation = Quaternion.Euler(RotationX, RotationY, 0);
            Orientation.rotation = Quaternion.Euler(0, RotationY, 0);
        }
        else
        {
            return;
        }
    }
}
