using TMPro;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private GameObject ballPrefab;
    private int score;

    void Start()
    {
        score = 0;
    }

    public void Score()
    {
        score += 1;
        scoreLabel.text = score.ToString();
        Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
