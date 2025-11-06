using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingTutorial : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("throwing")]
    public KeyCode throwKey = KeyCode.G;
    public float throwForce;
    public float throwUpwardForce;

    [Header("Trajectory Preview")]
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 30;
    public float trajectoryTimeStep = 0.1f;
    public Material trajectoryMaterial;
    
    [Header("Debug")]
    public bool showDebugInfo = true;

    private bool readyToThrow;
    private bool isAiming = false;

    private void Start()
    {
        readyToThrow = true;
        SetupTrajectoryLine();
    }

    private void SetupTrajectoryLine()
    {
        if (trajectoryLine == null)
        {
            GameObject trajectoryObj = new GameObject("TrajectoryLine");
            trajectoryLine = trajectoryObj.AddComponent<LineRenderer>();
        }
        
        trajectoryLine.positionCount = trajectoryPoints;
        trajectoryLine.startWidth = 0.05f;
        trajectoryLine.endWidth = 0.05f;
        trajectoryLine.useWorldSpace = true;
        trajectoryLine.enabled = false;
        
        if (trajectoryMaterial != null)
            trajectoryLine.material = trajectoryMaterial;
    }

    private void Update()
    {
        if (readyToThrow && totalThrows > 0)
        {
            if (Input.GetKey(throwKey))
            {
                if (!isAiming)
                {
                    isAiming = true;
                    ShowTrajectory();
                }
                UpdateTrajectory();
            }
            else if (Input.GetKeyUp(throwKey) && isAiming)
            {
                isAiming = false;
                HideTrajectory();
                Throw();
            }
        }
        
        if (!isAiming)
        {
            HideTrajectory();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw with neutral rotation
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.identity);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + Vector3.up * throwUpwardForce;

        // Debug logging
        if (showDebugInfo)
        {
            Debug.Log($"Cam Forward: {cam.transform.forward}");
            Debug.Log($"Force Direction: {forceDirection}");
            Debug.Log($"Throw Force: {forceToAdd}, Mass: {projectileRb.mass}, Drag: {projectileRb.drag}");
            Debug.Log($"Calculated Velocity: {forceToAdd / projectileRb.mass}");
            Debug.Log($"Start Position: {attackPoint.position}");
            Debug.Log($"Player Rotation: {transform.rotation.eulerAngles}");
        }

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows = totalThrows - 1;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void ShowTrajectory()
    {
        trajectoryLine.enabled = true;
    }

    private void HideTrajectory()
    {
        trajectoryLine.enabled = false;
    }

    private void UpdateTrajectory()
    {
        Vector3 startPosition = attackPoint.position;
        Vector3 startVelocity = CalculateThrowVelocity();
        
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * trajectoryTimeStep;
            Vector3 point = CalculateTrajectoryPoint(startPosition, startVelocity, time);
            trajectoryLine.SetPosition(i, point);
            
            // Stop trajectory if it hits something
            if (i > 0)
            {
                Vector3 previousPoint = trajectoryLine.GetPosition(i - 1);
                if (Physics.Linecast(previousPoint, point))
                {
                    // Set remaining points to the collision point
                    for (int j = i; j < trajectoryPoints; j++)
                    {
                        trajectoryLine.SetPosition(j, point);
                    }
                    break;
                }
            }
        }
    }

    private Vector3 CalculateThrowVelocity()
    {
        Vector3 forceDirection = cam.transform.forward;
        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        
        Vector3 forceToAdd = forceDirection * throwForce + Vector3.up * throwUpwardForce;
        
        // Get the mass from the projectile prefab to calculate actual velocity
        Rigidbody prefabRb = objectToThrow.GetComponent<Rigidbody>();
        float mass = prefabRb != null ? prefabRb.mass : 1f;
        
        if (showDebugInfo)
        {
            Debug.Log($"Force Direction: {forceDirection}");
            Debug.Log($"Force to Add: {forceToAdd}");
            Debug.Log($"Final Velocity: {forceToAdd / mass}");
        }
        
        // ForceMode.Impulse: velocity = force / mass
        return forceToAdd / mass;
    }

    private Vector3 CalculateTrajectoryPoint(Vector3 startPosition, Vector3 startVelocity, float time)
    {
        // Standard projectile motion: position = start + velocity*time + 0.5*gravity*time^2
        Vector3 position = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
        
        if (showDebugInfo && time == trajectoryTimeStep) // Log first point only
        {
            Debug.Log($"First trajectory point: Start={startPosition}, Velocity={startVelocity}, Time={time}, Position={position}");
        }
        
        return position;
    }

}
