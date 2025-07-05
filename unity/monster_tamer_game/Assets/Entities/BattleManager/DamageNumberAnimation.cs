using TMPro;
using UnityEngine;

public class DamageNumberAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    private TMP_Text tmp;
    private float time = 0;
    private Vector3 origin;
    private float offset = 0.2f;

    void Start()
    {
        tmp = GetComponent<TMP_Text>();
        origin = transform.position;
    }

    void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, heightCurve.Evaluate(time) + offset, 0);
        time += Time.deltaTime;
    }
}
