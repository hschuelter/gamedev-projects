using UnityEngine;
using UnityEngine.UI;

public class ResourceBarHUD : MonoBehaviour
{
    [SerializeField] Image barComponent;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = barComponent.GetComponent<RectTransform>();
    }

    public void UpdateResourceBar(float ratio, Color color)
    {
        rectTransform = barComponent.GetComponent<RectTransform>();
        ratio = Mathf.Clamp(ratio, 0f, 100f);
        rectTransform.SetRight(100f - ratio);
        barComponent.color = color;
    }

    public void SetRight(float right)
    {
        rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
    }
}
