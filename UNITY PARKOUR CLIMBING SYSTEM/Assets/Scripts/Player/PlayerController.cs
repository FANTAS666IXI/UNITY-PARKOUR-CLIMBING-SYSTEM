using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;

    private float currentWalkSpeed;
    private float currentRunSpeed;
    private float horizontalInput;
    private float verticalInput;
    private float moveAmount;
    private bool isMoving;
    private bool isRuning;
    private Vector3 moveInput;
    private Vector3 moveDir;
    private Quaternion targetRotation;
    private CameraController cameraController;
    private Animator animator;

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
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        currentWalkSpeed = walkSpeed;
        currentRunSpeed = runSpeed;
    }

    private void Update()
    {
        UpdateMovementInput();
        UpdateIsRuning();
        if (isMoving)
            MovePlayer();
        RotatePlayer();
        UpdateMoveAnimations();
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
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        isMoving = (0 < moveAmount);
        moveDir = cameraController.PlanarRotation * moveInput;
    }

    private void MovePlayer()
    {
        if (isRuning)
            transform.position += currentRunSpeed * Time.deltaTime * moveDir;
        else
            transform.position += currentWalkSpeed * Time.deltaTime * moveDir;
        targetRotation = Quaternion.LookRotation(moveDir);
        ConsoleLog("Player Moves.");
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateMoveAnimations()
    {
        if (isMoving)
        {
            if (isRuning)
                moveAmount = Mathf.Clamp(moveAmount, 0.2f, 1);
            else
                moveAmount = Mathf.Clamp(moveAmount, 0, 0.2f);
        }
        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public float GetRunSpeed()
    {
        return runSpeed;
    }

    public void SetCurrentWalkSpeed(float walkSpeed)
    {
        currentWalkSpeed = walkSpeed;
    }

    public void SetCurrentRunSpeed(float runSpeed)
    {
        currentRunSpeed = runSpeed;
    }

    private void ConsoleLog(string message)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor);
    }
}