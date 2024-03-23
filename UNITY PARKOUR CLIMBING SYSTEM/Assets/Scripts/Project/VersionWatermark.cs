using TMPro;
using UnityEngine;

public class VersionWatermark : MonoBehaviour
{
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
        waterwarkText.SetText("v0.2.0");
        waterwarkText.fontStyle = FontStyles.Bold;
        waterwarkText.color = new Color(1f, 1f, 1f, 0.4f);
        waterwarkText.alignment = TextAlignmentOptions.BottomRight;
    }
}