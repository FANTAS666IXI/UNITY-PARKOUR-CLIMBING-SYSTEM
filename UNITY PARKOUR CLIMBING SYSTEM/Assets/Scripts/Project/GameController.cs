using UnityEngine;
using System.Diagnostics;

public class GameController : MonoBehaviour
{
    [Header("Game Controller Settings")]
    public bool developerMode;
    public Material defaultMaterial;
    public Material developerMaterial;
    [Header("Console Log Settings")]
    public Color classColor;
    public bool consoleLog;
    public bool consoleLogSystem;

    private PlayerController player;
    private Renderer ground;

    private void Awake()
    {
        StartingGame();
        InitializeReferences();
        LockCursor();
    }

    private void StartingGame()
    {
        ConsoleLog("Starting Game...", true);
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
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.F12))
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
            ConsoleLog("Closing Game...", true);
            Application.Quit();
        }
    }

    private void ConsoleLog(string message, bool showFrame = false, int infoLevel = 0)
    {
        if (consoleLog)
            ConsoleLogSystem($"{message}", classColor, showFrame, infoLevel);
    }

    public void ConsoleLogSystem(string message, Color classColor, bool showFrame, int infoLevel, int traceFrame = 1)
    {
        if (consoleLogSystem)
        {
            StackTrace stackTrace = new();
            StackFrame stackFrame = stackTrace.GetFrame(traceFrame);
            string callingScript = stackFrame.GetMethod().DeclaringType.Name;
            string stringClassColor = ("#" + ColorUtility.ToHtmlStringRGBA(classColor));
            switch (infoLevel)
            {
                case 0:
                    if (showFrame)
                        UnityEngine.Debug.Log($"<b>[<color={stringClassColor}>{callingScript}</color>]: (FRM: {Time.frameCount}) {message}</b>");
                    else
                        UnityEngine.Debug.Log($"<b>[<color={stringClassColor}>{callingScript}</color>]: {message}</b>");
                    break;
                case 1:
                    if (showFrame)
                        UnityEngine.Debug.LogWarning($"<b>[<color={stringClassColor}>{callingScript}</color>]: (FRM: {Time.frameCount}) {message}</b>");
                    else
                        UnityEngine.Debug.LogWarning($"<b>[<color={stringClassColor}>{callingScript}</color>]: {message}</b>");
                    break;
                case 2:
                    if (showFrame)
                        UnityEngine.Debug.LogError($"<b>[<color={stringClassColor}>{callingScript}</color>]: (FRM: {Time.frameCount}) {message}</b>");
                    else
                        UnityEngine.Debug.LogError($"<b>[<color={stringClassColor}>{callingScript}</color>]: {message}</b>");
                    break;
            }
        }
    }
}