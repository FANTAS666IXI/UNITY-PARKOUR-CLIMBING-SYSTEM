using UnityEngine;
using System.Diagnostics;

public class GameController : MonoBehaviour
{
    public bool developerMode;
    public Material defaultMaterial;
    public Material developerMaterial;

    private PlayerController player;
    private Renderer ground;

    public Color classColor;
    public bool consoleLog;
    public bool consoleLogSystem;

    private void Awake()
    {
        StartingGame();
        InitializeReferences();
        LockCursor();
    }

    private void StartingGame()
    {
        ConsoleLog("Starting Game...");
    }

    private void InitializeReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ground = GameObject.Find("Ground").GetComponent<Renderer>();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        InitializeDeveloperMode();
    }

    private void InitializeDeveloperMode()
    {
        if (developerMode)
            EnableDeveloperMode();
        else
            DisableDeveloperMode();
    }

    private void EnableDeveloperMode()
    {
        developerMode = true;
        player.SetCurrentWalkSpeed(player.GetWalkSpeed() * 5);
        player.SetCurrentRunSpeed(player.GetRunSpeed() * 5);
        ground.material = developerMaterial;
        ConsoleLog("Developer Mode Enabled.");
    }

    private void DisableDeveloperMode()
    {
        developerMode = false;
        player.SetCurrentWalkSpeed(player.GetWalkSpeed());
        player.SetCurrentRunSpeed(player.GetRunSpeed());
        ground.material = defaultMaterial;
        ConsoleLog("Developer Mode Disabled.");
    }

    private void Update()
    {
        UpdateDeveloperMode();
        ExitGame();
    }

    private void UpdateDeveloperMode()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (developerMode)
                DisableDeveloperMode();
            else
                EnableDeveloperMode();
        }
    }

    private void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ConsoleLog("Closing Game...");
            Application.Quit();
        }
    }

    private void ConsoleLog(string message)
    {
        if (consoleLog)
            ConsoleLogSystem($"{message}", classColor);
    }

    public void ConsoleLogSystem(string message, Color classColor, int frame = 1)
    {
        if (consoleLogSystem)
        {
            StackTrace stackTrace = new();
            StackFrame stackFrame = stackTrace.GetFrame(frame);
            string callingScript = stackFrame.GetMethod().DeclaringType.Name;
            string stringClassColor = ("#" + ColorUtility.ToHtmlStringRGBA(classColor));
            UnityEngine.Debug.Log($"<b>[<color={stringClassColor}>{callingScript}</color>]: {message}</b>");
        }
    }
}