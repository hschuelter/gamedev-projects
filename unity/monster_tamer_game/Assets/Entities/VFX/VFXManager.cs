using TMPro;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }
    public GameObject vfxHitPrefab;
    public GameObject vfxHealPrefab;
    public GameObject canvas;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowVFX(GameObject vfxPrefab, Vector3 position)
    {
        var popup = Instantiate(vfxPrefab, position, Quaternion.identity, canvas.transform);
        Destroy(popup, 1f);
    }
}
