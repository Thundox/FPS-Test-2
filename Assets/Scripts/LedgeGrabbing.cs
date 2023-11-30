using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement pm;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;

    public float minTimeOnLedge;
    private float TimeOnledge;

    public bool holding;

    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform LastLedge;
    private Transform currLedge;

    private RaycastHit ledgeHit;

    private void Update()
    {
        LedgeDetection();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("vertical");
        bool anyInputKeyPressed = horizontalInput !=0 || verticalInput !=0;

        // SubState 1 - Holding onto ledge
        if (holding)
        {
            FreezeRigidBodyOnLedge();

            TimeOnledge += Time.deltaTime;

            if (TimeOnledge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();
        }
    }

    private void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if (!ledgeDetected) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if (ledgeHit.transform == LastLedge) return;

        if (distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }

    private void EnterLedgeHold()
    {
        holding = true;

        currLedge =ledgeHit.transform;
        LastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void FreezeRigidBodyOnLedge()
    {

    }

    private void ExitLedgeHold()
    {

    }
}
