using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;

    private float horizontalInput;
    private float verticalInput;
    private float movingAmount;
    private bool isMoving;
    private bool isRuning;
    private Vector3 moveInput;
    private Vector3 moveDir;
    private Quaternion targetRotation;
    private CameraController cameraController;

    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

    private void Awake()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        UpdateMovementInput();
        UpdateIsRuning();
        if (isMoving)
            MovePlayer();
        RotatePlayer();
    }

    private void UpdateIsRuning()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRuning = true;
            ConsoleLog("Player Starts Runing.");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRuning = false;
            ConsoleLog("Player Stops Runing.");
        }
    }

    private void UpdateMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        moveInput = (new Vector3(horizontalInput, 0, verticalInput)).normalized;
        movingAmount = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        isMoving = (0 < movingAmount);
        moveDir = cameraController.PlanarRotation * moveInput;
    }

    private void MovePlayer()
    {
        if (isRuning)
            transform.position += runSpeed * Time.deltaTime * moveDir;
        else
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        targetRotation = Quaternion.LookRotation(moveDir);
        ConsoleLog("Player Moves");
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ConsoleLog(string message)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor);
    }
}