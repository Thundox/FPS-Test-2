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
    //public float JumpHeight;

    public float JumpForce;
    public float JumpCooldown;
    public float AirMultiplier;
    public bool ReadyToJump;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

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
        ReadyToJump = true;

    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);
        
        MyInput();
        SpeedControl();

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
        if (Input.GetKey(jumpkey) && ReadyToJump && Grounded)
        {
            ReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }
    
    private void MovePlayer()
    {
        MoveDirection = OrientationTransform.forward * VerticalInput + OrientationTransform.right * HorizontalInput;
        // Replace 10f with acceleration variable

        // On Ground
        if (Grounded)
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);

        // In Air
        else if(!Grounded)
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed * 10f * AirMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);

        // Limit Velocity if needed
        if (flatVel.magnitude > MovementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MovementSpeed;
            PlayerRigidbody.velocity = new Vector3(limitedVel.x, PlayerRigidbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);

        PlayerRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;
    }
}
