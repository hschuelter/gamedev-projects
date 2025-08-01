using TMPro;
using UnityEngine;

public class CharacterExpHUD : MonoBehaviour
{
    float EXP_GAIN_DURATION = 2;

    public Stats characterStats;
    [SerializeField] ResourceBarHUD expBarHUD;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;

    float expCap;
    float rate = 1;
    private float targetEXP;
    private float currentExp;
    public bool isOk = false;
    public bool isReady = false;

    private void Update()
    {
        if (!isReady) return;

        if (currentExp >= Mathf.Clamp(targetEXP, 0, expCap))
        {
            CheckExp();
            return;
        }

        currentExp += Time.deltaTime * rate;
        UpdateExpBar();
    }

    public void Draw(int expGained)
    {
        isReady = false;
        UpdateExpCap();

        currentExp = characterStats.currentExp;
        targetEXP = currentExp + expGained;
        rate = expCap / EXP_GAIN_DURATION;

        UpdateName();
        UpdateLevel();
        UpdateExpBar();
    }
    public void UpdateExpBar()
    {
        var ratio = currentExp / expCap;
        expText.text = $"{Mathf.FloorToInt(currentExp)}";

        var barMax = 210f;
        expBarHUD.UpdateResourceBar(barMax, barMax * ratio);
    }
    public void UpdateName()
    {
        nameText.text = characterStats.nickname;
    }
    public void UpdateLevel()
    {
        levelText.text = $"Lv.{characterStats.level}";
    }

    public void CheckExp()
    {
        if (currentExp >= expCap)
        {
            characterStats.LevelUp();
            currentExp = 0;
            targetEXP -= expCap;

            UpdateExpCap();
            UpdateLevel();
            UpdateExpBar();
        }
        else
        {
            characterStats.UpdateExp(Mathf.FloorToInt(targetEXP));
            isOk = true;
        }
    }

    private void UpdateExpCap()
    {
        expCap = ExpTables.GetExpForLevel(characterStats.level);
    }
}
