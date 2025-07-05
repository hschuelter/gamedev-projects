using TMPro;
using UnityEngine;

public class DamageNumbersManager : MonoBehaviour
{
    public static DamageNumbersManager Instance { get; private set; }
    public GameObject popupPrefab;
    public GameObject canvas;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        var popup = Instantiate(popupPrefab, position, Quaternion.identity, canvas.transform);
        popup.GetComponent<TMP_Text>().text = damage.ToString();
        //Debug.Log($"[DamageNumbersManager] {damage} at ({position.x}, {position.y}) -> {popup}");

        Destroy(popup, 1f);
    }
}
