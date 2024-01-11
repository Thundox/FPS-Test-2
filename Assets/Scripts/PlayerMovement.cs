using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement")]
    private float MovementSpeed;
    public float WalkSpeed;
    public float SprintSpeed;
    public float slideSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    //public float wallrunSpeed;
    public float climbSpeed;
    //public float airMinSpeed;

    // 
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

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
    public bool grounded;

    [Header("Slope Handling")]
    public float MaxSlopeAngle;
    private RaycastHit SlopeHit;
    private bool ExitingSlope;

    [Header("References")]
    public Climbing climbingScript;

    Rigidbody rb;
    public Transform OrientationTransform;

    float HorizontalInput;
    float VerticalInput;

    Vector3 MoveDirection;

    public MovementState State;
    public enum MovementState
    {
        unlimited,
        freeze,
        Walking,
        Sprinting,
        climbing,
        Crouching,
        sliding,
        Air
    }

    // Movement States
    public bool unlimited;
    public bool freeze;
    public bool restricted;

    // sliding bool moved from sliding script to PlayerMovement
    public bool sliding;
    public bool climbing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ReadyToJump = true;
        StartYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround); // (Bug fix jumping up slopes) + 0.2 should be 0.1 while crouching
        
        MyInput();
        SpeedControl();
        StateHandler();

        // Handle Drag
        if (grounded) 
        {
            rb.drag = GroundDrag;
        }
        else 
        {
            rb.drag = 0;
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
        if (Input.GetKey(jumpkey) && ReadyToJump && grounded)
        {
            ReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), JumpCooldown);
        }

        // Start Crouch
        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, CrouchYScale, transform.localScale.z);
         if (grounded) rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
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
        // Mode - freeze 
        if (freeze)
        {
            State = MovementState.freeze;
            rb.velocity = Vector3.zero;
        }
        // Mode - unlimited 
        if (unlimited)
        {
            State = MovementState.unlimited;
            MovementSpeed = 999f;
            return;
        }
        // Mode - Climbing 
        else if (climbing)
        {
            State = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }
        // Mode - Sliding
        else if (sliding)
        {
            State = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = SprintSpeed;

        }
        // Mode - Crouching
        else if (Input.GetKey(crouchkey))
        {
            State = MovementState.Crouching;
         if (grounded) desiredMoveSpeed = CrouchSpeed;
        }
        
            
        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintkey))
        {
            State = MovementState.Sprinting;
            desiredMoveSpeed = SprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            State = MovementState.Walking;
            desiredMoveSpeed = WalkSpeed;
        }

        // Mode - Air
        else
        {
            State = MovementState.Air;
        }

        // check if desiredMoveSpeed has changed drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && MovementSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            MovementSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed (currently MovementSpeed) to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - MovementSpeed);
        float startValue = MovementSpeed;

        while (time < difference)
        {
            MovementSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                // [Uppercase camel casing] SlopeHit
                float slopeAngle = Vector3.Angle(Vector3.up, SlopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        MovementSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (restricted) return;
        if (climbingScript.exitingWall) return; 

        // Calculate Movement Direction
        MoveDirection = OrientationTransform.forward * VerticalInput + OrientationTransform.right * HorizontalInput;
        // Replace 10f with acceleration variable

        // On Slope
        if (OnSlope() && !ExitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(MoveDirection) * MovementSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > -5)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // On Ground
        else if (grounded)
        rb.AddForce(MoveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);

        // In Air
        else if(!grounded)
        rb.AddForce(MoveDirection.normalized * MovementSpeed * 10f * AirMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // Limiting Speed on Slope
        if (OnSlope() && !ExitingSlope)
        {
            if (rb.velocity.magnitude > MovementSpeed)
            rb.velocity = rb.velocity.normalized * MovementSpeed;
        }

        // Limiting Speed on Ground
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit Velocity if needed
            if (flatVel.magnitude > MovementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * MovementSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        
    }

    private void Jump()
    {
        ExitingSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
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
