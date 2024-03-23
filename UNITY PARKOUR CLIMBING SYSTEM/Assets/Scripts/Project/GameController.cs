using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool developerMode;

    public Color classColor;
    public bool consoleLog;
    public bool consoleLogSystem;

    private void Awake()
    {
        StartingGame();
        InitializeVariables();
        LockCursor();
    }

    private void StartingGame()
    {
        ConsoleLog("Starting Game...");
        ConsoleLog("Starting Main Scene.");
    }

    private void InitializeVariables()
    {
        EnableDeveloperMode();
    }

    private void EnableDeveloperMode()
    {
        developerMode = true;
        ConsoleLog("Developer Mode Enabled.");
    }

    private void DisableDeveloperMode()
    {
        developerMode = true;
        ConsoleLog("Developer Mode Disabled.");
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        DeveloperMode();
        ScenesManagement();
        ExitGame();
    }

    private void DeveloperMode()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (!developerMode)
                EnableDeveloperMode();
            else
                DisableDeveloperMode();
        }
    }

    private void ScenesManagement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConsoleLog("Loading Test Scene...");
            SceneManager.LoadScene("Test Scene");
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