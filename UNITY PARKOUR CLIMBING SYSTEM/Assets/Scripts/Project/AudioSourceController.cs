using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public int volume;
    public int volumeModifier;
    public int minVolume;
    public int maxVolume;
    public bool muted;

    private float trueVolume;
    private AudioSource audioSource;

    public Color classColor;
    public bool consoleLog;
    private GameController gameController;

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
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            muted = false;
            volume += volumeModifier;
            SetVolume();
        }
    }

    private void CheckDecreaseVolume()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            volume -= volumeModifier;
            SetVolume();
        }
    }

    private void CheckMuteVolume()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            muted = !muted;
            SetVolume();
        }
    }

    private void ConsoleLog(string message)
    {
        if (consoleLog)
            gameController.ConsoleLogSystem($"{message}", classColor);
    }
}