using TMPro;
using UnityEngine;

public class VersionWatermark : MonoBehaviour
{
    [Header("Version Watermark Settings")]
    public bool show;

    private TMP_Text waterwarkText;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        waterwarkText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        SetVersionWatermark();
    }

    private void SetVersionWatermark()
    {
        waterwarkText.SetText(GetVersionText());
        waterwarkText.fontStyle = FontStyles.Bold;
        waterwarkText.color = new Color(1f, 1f, 1f, 0.4f);
        waterwarkText.alignment = TextAlignmentOptions.BottomRight;
    }

    private string GetVersionText()
    {
        if (show)
            return Alias.GetProjectVersion();
        else
            return "";
    }
}