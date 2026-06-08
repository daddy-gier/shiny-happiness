using UnityEngine;
using TMPro;

public class NH_InteractionPromptWidget : MonoBehaviour
{
    public TMP_Text promptText;
    public CanvasGroup canvasGroup;

    public void Show(string text)
    {
        if (promptText) promptText.text = text;
        if (canvasGroup) canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        if (canvasGroup) canvasGroup.alpha = 0f;
    }
}
