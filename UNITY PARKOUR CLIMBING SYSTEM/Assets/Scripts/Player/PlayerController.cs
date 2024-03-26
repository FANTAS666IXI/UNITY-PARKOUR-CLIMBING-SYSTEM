using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    [Header("Check Ground Settings")]
    [SerializeField] float checkGroundRadius;
    [SerializeField] Vector3 checkGroundOffset;
    [SerializeField] LayerMask groundLayer;
    [Header("Console Log Settings")]
    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

    private float currentWalkSpeed;
    private float currentRunSpeed;
    private float horizontalInput;
    private float verticalInput;
    private float moveAmount;
    private float ySpeed;
    private bool isMoving;
    private bool isRuning;
    private bool isGrounded;
    private Vector3 moveInput;
    private Vector3 moveDir;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private CharacterController characterController;
    private CameraController cameraController;
    private Animator animator;

    private void Awake()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(checkGroundOffset), checkGroundRadius);
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
        UpdateIsGrounded();
        UpdateIsRuning();
        MovePlayer();
        RotatePlayer();
        UpdateMoveAnimations();
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

    private void MovePlayer()
    {
        if (isRuning)
            velocity = moveDir * currentRunSpeed;
        else
            velocity = moveDir * currentWalkSpeed;
        if (isGrounded)
            ySpeed = -0.5f;
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);
        if (isMoving)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }
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

    private void UpdateIsGrounded()
    {
        if (Physics.CheckSphere(transform.TransformPoint(checkGroundOffset), checkGroundRadius, groundLayer))
            isGrounded = true;
        else
            isGrounded = false;
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

    private void ConsoleLog(string message, bool showFrame = false, int infoLevel = 0)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor, showFrame, infoLevel);
    }
}