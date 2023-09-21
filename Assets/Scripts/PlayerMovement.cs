using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;

    public float JumpHeight;
    public bool ReadyToJump;

    public Rigidbody PlayerRigidbody;
    public Transform OrientationTransform;

    float HorizontalInput;
    public float VerticalInput;

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
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        MoveDirection = OrientationTransform.forward * VerticalInput + OrientationTransform.right * HorizontalInput;
        PlayerRigidbody.AddForce(MoveDirection.normalized * MovementSpeed, ForceMode.Force);

    }
}
