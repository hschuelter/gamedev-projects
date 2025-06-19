using UnityEngine;
using UnityEngine.UI;

public class ResourceBarHUD : MonoBehaviour
{
    [SerializeField] Image barComponent;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = barComponent.GetComponent<RectTransform>();
    }

    void Update()
    {

    }

    public void UpdateResourceBar(float ratio, Color color)
    {
        ratio = Mathf.Clamp(ratio, 0f, 100f);
        rectTransform.SetRight(100f - ratio);
        barComponent.color = color;
    }
}
