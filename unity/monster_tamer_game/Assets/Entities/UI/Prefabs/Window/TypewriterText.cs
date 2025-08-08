using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TypewriterText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text textField;
    [SerializeField] private float timer;
    [SerializeField] private float charactersPerSecond = 20f;

    [Header("Debug Info")]
    [SerializeField] private bool isTyping;
    [SerializeField] private int currentCharIndex;
    
    public bool IsTyping => isTyping;
    private float timerReset => 1 / charactersPerSecond;
    private string targetText;

    void Awake()
    {
        targetText = "";
        currentCharIndex = 0;
        timer = timerReset;
        isTyping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetText.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            HandleCurrentChar();
            textField.text = targetText.Substring(0, currentCharIndex);
            timer = timerReset;
        }
    }

    public void SetText(string text)
    {
        Debug.Log($"SetText");
        targetText = text;
        textField.text = "";
        currentCharIndex = 0;
        isTyping = true;
    }
    public void CompleteTyping()
    {
        isTyping = false;
    }

    private void HandleCurrentChar()
    {
        if (!isTyping)
        {
            currentCharIndex = targetText.Length;
            return;
        }

        currentCharIndex++;
        currentCharIndex = Mathf.Clamp(currentCharIndex, 0, targetText.Length);

        if (currentCharIndex == targetText.Length) isTyping = false;
    }
}
