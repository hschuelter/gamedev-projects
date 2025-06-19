using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameObject descriptionWindow;
    [SerializeField] Color healthyColor;
    [SerializeField] Color attentionColor;
    [SerializeField] Color dangerColor;

    private TMP_Text descriptionText;

    private void Start()
    {
        descriptionText = descriptionWindow.GetComponentInChildren<TMP_Text>();
        descriptionWindow.SetActive(false);
    }
    public void UpdateHUD(PartyMemberHUDManager characterHUDManager) 
    {
        characterHUDManager.UpdateHealth();
        characterHUDManager.UpdateMana();
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
    public void ShowDescription()
    {
        descriptionWindow.SetActive(true);
    }
    public void HideDescription()
    {
        descriptionWindow.SetActive(false);
    }
}
