using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    [SerializeField] int minDistanceUnits;
    [SerializeField] int maxDistanceUnits;
    [SerializeField] float minVerticalAngle;
    [SerializeField] float maxVerticalAngle;
    [SerializeField] Vector2 framingOffSet;

    private float invertXVal;
    private float invertYVal;
    private int distanceUnits;
    private Vector3 targetDistance;
    private float rotationX;
    private float rotationY;
    private Quaternion targetRotation;
    private Vector3 targetFocus;

    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

    private void Awake()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        distanceUnits = 6;
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;
        ConsoleLog("Initial Camera Distance = " + distanceUnits + " Units.");
    }

    private void Update()
    {
        UpdateTargetDistance();
        UpdateTargetRotation();
        UpdateTargetLocation();
        UpdateTargetOrientation();
    }

    private void UpdateTargetDistance()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (distanceUnits > minDistanceUnits)
            {
                distanceUnits--;
                ConsoleLog("Current Camera Distance = " + distanceUnits + " Units.");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.KeypadMinus) & distanceUnits < maxDistanceUnits)
            {
                distanceUnits++;
                ConsoleLog("Current Camera Distance = " + distanceUnits + " Units.");
            }
        }
        targetDistance = new Vector3(0, 0, distanceUnits);
    }

    private void UpdateTargetRotation()
    {
        rotationY += Input.GetAxis("Mouse X") * invertYVal * rotationSpeed;
        rotationX -= Input.GetAxis("Mouse Y") * invertXVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    private void UpdateTargetLocation()
    {
        targetFocus = followTarget.position + new Vector3(framingOffSet.x, framingOffSet.y);
        transform.position = targetFocus - (targetRotation * targetDistance);
    }

    private void UpdateTargetOrientation()
    {
        transform.rotation = targetRotation;
    }

    private void ConsoleLog(string message)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor);
    }
}