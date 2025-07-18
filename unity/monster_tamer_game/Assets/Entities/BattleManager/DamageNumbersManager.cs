using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DamageNumbersManager : MonoBehaviour
{
    public static DamageNumbersManager Instance { get; private set; }
    public GameObject popupPrefab;
    public GameObject canvas;

    [SerializeField] Color normalColor = new Color(1, 1, 1, 1);
    [SerializeField] Color criticalHitColor = new Color(1, 1, 1, 1);
    [SerializeField] Color healColor;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(float damage, Vector3 position, bool isCritical = false)
    {
        var popup = Instantiate(popupPrefab, position, Quaternion.identity, canvas.transform);
        Color color = isCritical ? criticalHitColor : normalColor;

        if (damage < 0)
        {
            damage = -damage;
            color = healColor;
        }

        string damageText = damage.ToString();

        if (damage == 0)
        {
            damageText = "Miss";
        }

        popup.GetComponent<TMP_Text>().text = damageText;
        popup.GetComponent<TMP_Text>().faceColor = color;

        Destroy(popup, 1f);
    }
}
