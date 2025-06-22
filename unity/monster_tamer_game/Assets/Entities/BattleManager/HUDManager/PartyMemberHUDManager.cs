using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PartyMemberHUDManager : MonoBehaviour
{
    public GameObject partyMember;
    [SerializeField] ResourceBarHUD healthBarHUD;
    [SerializeField] ResourceBarHUD manaBarHUD;
    [SerializeField] Color healthyColor;
    [SerializeField] Color attentionColor;
    [SerializeField] Color dangerColor;
    [SerializeField] Color whiteColor;
    [SerializeField] Color manaColor;

    private TMP_Text nameText;
    private TMP_Text healthText;
    private TMP_Text manaText;
    private MonsterStats stats;

    void Start()
    {
        Draw();
    }

    public void Draw()
    {
        stats = partyMember.GetComponent<MonsterStats>();
        stats.characterHUDManager = this;
        foreach (Transform child in transform)
        {
            if (child.name == "Name")
            {
                nameText = child.GetComponentInChildren<TMP_Text>();
            }
            if (child.name == "Health")
            {
                healthText = child.GetComponentInChildren<TMP_Text>();
            }
            if (child.name == "Mana")
            {
                manaText = child.GetComponentInChildren<TMP_Text>();
            }
        }

        nameText.text = stats.nickname;
        UpdateHealth();
        UpdateMana();
    }

    public void UpdateHealth()
    {
        var ratio = 100 * stats.currentHealth / stats.maxHealth;

        Color currentColor = healthyColor;
        if (ratio < 50) currentColor = attentionColor;
        if (ratio < 25) currentColor = dangerColor;


        healthText.text = $"{stats.currentHealth}";
        if (ratio == 100) healthText.color = healthyColor;
        else healthText.color = whiteColor;

        healthBarHUD.UpdateResourceBar(ratio, currentColor);
    }
    public void UpdateMana()
    {
        var ratio = 100 * stats.currentMana / stats.maxMana;

        manaText.text = $"{stats.currentMana}";
        if (ratio == 100) manaText.color = manaColor;
        else manaText.color = whiteColor;

        manaBarHUD.UpdateResourceBar(ratio, manaColor);
    }

}
