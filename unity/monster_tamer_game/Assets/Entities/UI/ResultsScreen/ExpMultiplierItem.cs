using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ExpMultiplierItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    public string Description;
    public float Multiplier;
    public bool IsActive = false;
    private void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>();
    }
    public void Show(bool value)
    {
        _label.text = $"{Description} x{Multiplier}";
        transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(value);
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
}
