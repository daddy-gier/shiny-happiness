using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NH_InteractionPromptUI : MonoBehaviour
{
    public GameObject promptRoot;
    public TMP_Text verbText;
    public TMP_Text nameText;

    void Awake()
    {
        if (promptRoot) promptRoot.SetActive(false);
    }

    public void Show(string verb, string objName)
    {
        if (promptRoot) promptRoot.SetActive(true);
        if (verbText) verbText.text = $"[E] {verb}";
        if (nameText) nameText.text = objName;
    }

    public void Hide()
    {
        if (promptRoot) promptRoot.SetActive(false);
    }
}
