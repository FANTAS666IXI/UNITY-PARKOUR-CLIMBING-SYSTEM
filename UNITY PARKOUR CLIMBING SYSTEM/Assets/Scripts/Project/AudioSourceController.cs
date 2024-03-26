using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [Header("Audio Source Settings")]
    public int volume;
    public int volumeModifier;
    public int minVolume;
    public int maxVolume;
    public bool muted;
    [Header("Console Log Settings")]
    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

    private float trueVolume;
    private AudioSource audioSource;

    private void Awake()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        SetVolume();
    }
    private void SetVolume()
    {
        volume = Mathf.Clamp(volume, minVolume, maxVolume);
        trueVolume = (float)volume / 10;
        if (!muted)
        {
            audioSource.volume = trueVolume;
            ConsoleLog("Audio Unmuted.");
        }
        else
        {
            audioSource.volume = 0;
            ConsoleLog("Audio Muted.");
        }
        ConsoleLog("Current Volume = " + trueVolume + ".");
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        CheckIncreaseVolume();
        CheckDecreaseVolume();
        CheckMuteVolume();
    }

    private void CheckIncreaseVolume()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.F3))
        {
            muted = false;
            volume += volumeModifier;
            SetVolume();
        }
    }

    private void CheckDecreaseVolume()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.F1))
        {
            volume -= volumeModifier;
            SetVolume();
        }
    }

    private void CheckMuteVolume()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply) || Input.GetKeyDown(KeyCode.F2))
        {
            muted = !muted;
            SetVolume();
        }
    }

    private void ConsoleLog(string message, bool showFrame = false, int infoLevel = 0)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor, showFrame, infoLevel);
    }
}