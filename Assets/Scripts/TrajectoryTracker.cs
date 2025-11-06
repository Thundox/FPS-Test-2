using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracker : MonoBehaviour
{
    public LineRenderer actualTrajectoryLine;
    public Material actualTrajectoryMaterial;
    public Color trajectoryColor = Color.red;
    
    private List<Vector3> trajectoryPoints = new List<Vector3>();
    private float recordInterval = 0.1f;
    private float lastRecordTime;
    
    void Start()
    {
        SetupActualTrajectoryLine();
        lastRecordTime = Time.time;
        trajectoryPoints.Add(transform.position);
    }
    
    void Update()
    {
        // Record position every interval
        if (Time.time - lastRecordTime >= recordInterval)
        {
            trajectoryPoints.Add(transform.position);
            UpdateActualTrajectoryLine();
            lastRecordTime = Time.time;
        }
    }
    
    private void SetupActualTrajectoryLine()
    {
        if (actualTrajectoryLine == null)
        {
            GameObject trajectoryObj = new GameObject("ActualTrajectoryLine");
            actualTrajectoryLine = trajectoryObj.AddComponent<LineRenderer>();
        }
        
        
        actualTrajectoryLine.startWidth = 0.03f;
        actualTrajectoryLine.endWidth = 0.03f;
        actualTrajectoryLine.useWorldSpace = true;
        actualTrajectoryLine.startColor = trajectoryColor;
        actualTrajectoryLine.endColor = trajectoryColor;


        if (actualTrajectoryMaterial != null)
            actualTrajectoryLine.material = actualTrajectoryMaterial;
    }
    
    private void UpdateActualTrajectoryLine()
    {
        actualTrajectoryLine.positionCount = trajectoryPoints.Count;
        actualTrajectoryLine.SetPositions(trajectoryPoints.ToArray());
    }
    
    void OnDestroy()
    {
        // Keep the line visible for a few seconds after grenade explodes
        if (actualTrajectoryLine != null)
        {
            Destroy(actualTrajectoryLine.gameObject, 5f);
        }
    }
}