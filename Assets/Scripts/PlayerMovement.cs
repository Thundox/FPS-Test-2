using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement")]
    private float MovementSpeed;
    public float WalkSpeed;
    public float SprintSpeed;
    public float slideSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float GroundDrag;
    //public float JumpHeight;

    [Header("Jumping")]
    public float JumpForce;
    public float JumpCooldown;
    public float AirMultiplier;
    public bool ReadyToJump;

    [Header("Crouching")]
    public float CrouchSpeed;
    public float CrouchYScale;
    private float StartYScale;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode sprintkey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;
    bool Grounded;

    [Header("Slope Handling")]
    public float MaxSlopeAngle;
    private RaycastHit SlopeHit;
    private bool ExitingSlope;

    Rigidbody PlayerRigidbody;
    public Transform OrientationTransform;

    float HorizontalInput;
    float VerticalInput;

    Vector3 MoveDirection;

    public MovementState State;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        sliding,
        Air
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerRigidbody.freezeRotation = true;
        ReadyToJump = true;
        StartYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround); // (Bug fix jumping up slopes) + 0.2 should be 0.1 while crouching
        
        MyInput();
        SpeedControl();
        StateHandler();

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

        // Start Crouch
        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, CrouchYScale, transform.localScale.z);
         if (Grounded) PlayerRigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            PlayerHeight = PlayerHeight * 0.5f;
        }

        // Stop Crouch
        if (Input.GetKeyUp(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
            PlayerHeight = PlayerHeight * 2f;
        }
    }
    
    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchkey))
        {
            State = MovementState.Crouching;
         if (Grounded) MovementSpeed = CrouchSpeed;
        }
        
            
        // Mode - Sprinting
        else if (Grounded && Input.GetKey(sprintkey))
        {
            State = MovementState.Sprinting;
            MovementSpeed = SprintSpeed;
        }

        // Mode - Walking
        else if (Grounded)
        {
            State = MovementState.Walking;
            MovementSpeed = WalkSpeed;
        }

        // Mode - Air
        else
        {
            State = MovementState.Air;
        }
    }

    private void MovePlayer()
    {
        // Calculate Movement Direction
        MoveDirection = OrientationTransform.forward * VerticalInput + OrientationTransform.right * HorizontalInput;
        // Replace 10f with acceleration variable

        // On Slope
        if (OnSlope() && !ExitingSlope)
        {
            PlayerRigidbody.AddForce(GetSlopeMoveDirection(MoveDirection) * MovementSpeed * 20f, ForceMode.Force);

            if (PlayerRigidbody.velocity.y > 0)
                PlayerRigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // On Ground
        if (Grounded)
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);

        // In Air
        else if(!Grounded)
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed * 10f * AirMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        PlayerRigidbody.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // Limiting Speed on Slope
        if (OnSlope() && !ExitingSlope)
        {
            if (PlayerRigidbody.velocity.magnitude > MovementSpeed)
            PlayerRigidbody.velocity = PlayerRigidbody.velocity.normalized * MovementSpeed;
        }

        // Limiting Speed on Ground
        else
        {
            Vector3 flatVel = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);

            // Limit Velocity if needed
            if (flatVel.magnitude > MovementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * MovementSpeed;
                PlayerRigidbody.velocity = new Vector3(limitedVel.x, PlayerRigidbody.velocity.y, limitedVel.z);
            }
        }
        
    }

    private void Jump()
    {
        ExitingSlope = true;
        // reset y velocity
        PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);

        PlayerRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;

        ExitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, PlayerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        return false;

    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, SlopeHit.normal).normalized;
    }
}
