using TMPro;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }
    public GameObject vfxPrefab;
    public GameObject canvas;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowVFX(Vector3 position)
    {
        var popup = Instantiate(vfxPrefab, position, Quaternion.identity, canvas.transform);
        Destroy(popup, 1f);
    }
}
