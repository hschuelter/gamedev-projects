using TMPro;
using UnityEngine;

public class DamageNumbersManager : MonoBehaviour
{
    public static DamageNumbersManager Instance { get; private set; }
    public GameObject popupPrefab;
    public GameObject canvas;

    [SerializeField] Color normalColor = new Color(1, 1, 1, 1);
    [SerializeField] Color healColor;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        var popup = Instantiate(popupPrefab, position, Quaternion.identity, canvas.transform);
        Color color = normalColor;

        if (damage < 0)
        {
            damage = -damage;
            color = healColor;
        }

        popup.GetComponent<TMP_Text>().text = damage.ToString();
        popup.GetComponent<TMP_Text>().faceColor = color;

        Destroy(popup, 1f);
    }
}
