using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform followTarget;
    [SerializeField] int distanceUnits;
    [SerializeField] int minDistanceUnits;
    [SerializeField] int maxDistanceUnits;
    [SerializeField] float rotationSpeed;
    [SerializeField] float minVerticalAngle;
    [SerializeField] float maxVerticalAngle;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    [SerializeField] Vector2 framingOffSet;
    [Header("Console Log Settings")]
    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

    private float invertXVal;
    private float invertYVal;
    private float rotationX;
    private float rotationY;
    private Vector3 targetDistance;
    private Vector3 targetFocus;
    private Quaternion targetRotation;

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
        ConsoleLogCameraDistance();
    }

    private void InitializeVariables()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (distanceUnits > minDistanceUnits)
            {
                distanceUnits--;
                ConsoleLogCameraDistance();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) & distanceUnits < maxDistanceUnits)
            {
                distanceUnits++;
                ConsoleLogCameraDistance();
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

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

    private void ConsoleLogCameraDistance()
    {
        ConsoleLog("Current Camera Distance = " + distanceUnits + " Units.");
    }

    private void ConsoleLog(string message, bool showFrame = false, int infoLevel = 0)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor, showFrame, infoLevel);
    }
}