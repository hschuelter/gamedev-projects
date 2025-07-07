using TMPro;
using UnityEngine;

public class CommandDescriptionWindow : MonoBehaviour
{
    private TMP_Text descriptionText;

    public void Setup()
    {
        descriptionText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateText(string text)
    {
        descriptionText.text = text;
    }

    public void ShowWindow(bool value)
    {
        gameObject.SetActive(value);
    }
}
