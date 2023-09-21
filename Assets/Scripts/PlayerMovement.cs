using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement")]
    public float MovementSpeed;
    public float GroundDrag;
    public float JumpHeight;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;
    bool Grounded;

    Rigidbody PlayerRigidbody;
    public Transform OrientationTransform;

    float HorizontalInput;
    float VerticalInput;

    Vector3 MoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerRigidbody.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);
        
        MyInput();
        
        // Handle Drag
        if (Grounded) 
        {
            PlayerRigidbody.drag = GroundDrag;
        }
        else 
        {
            PlayerRigidbody.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

    }
    
    private void MovePlayer()
    {
        MoveDirection = OrientationTransform.forward * VerticalInput + OrientationTransform.right * HorizontalInput;
        // Replace 10f with acceleration variable
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);

    }
}
