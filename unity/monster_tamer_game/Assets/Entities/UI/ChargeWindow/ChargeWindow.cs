using UnityEditor.Search;
using UnityEngine;

public class ChargeWindow : MonoBehaviour
{
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject charge;
    [SerializeField] private GameObject reckless;

    public void UpdateUI(int value)
    {
        normal.SetActive(value >= 2);
        charge.SetActive(value >= 3);
        reckless.SetActive(value >= 4);
    }

    public void ShowWindow(bool value)
    {
        gameObject.SetActive(value);
    }
}
