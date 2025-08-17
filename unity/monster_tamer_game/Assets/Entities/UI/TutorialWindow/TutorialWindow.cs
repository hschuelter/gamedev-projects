using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    [Header("Tutorial Components")]
    [SerializeField] private TMP_Text _subtitle;
    [SerializeField] private TMP_Text _content;
    [SerializeField] private Image _imageA;
    [SerializeField] private Image _imageB;

    [Header("Tutorial Data")]
    [SerializeField] private List<TutorialContent> _tutorialContent;


    [Header("Debugging")]
    [SerializeField] private int _currentIndex = 0;


    void Start()
    {
        _currentIndex = 0;
        LoadTutorial(_tutorialContent.ElementAt(_currentIndex));
    }

    public void Next()
    {
        _currentIndex = Mathf.Clamp(_currentIndex + 1, 0, _tutorialContent.Count-1);
        LoadTutorial(_tutorialContent.ElementAt(_currentIndex));
    }
    public void Back()
    {
        _currentIndex = Mathf.Clamp(_currentIndex - 1, 0, _tutorialContent.Count - 1);
        LoadTutorial(_tutorialContent.ElementAt(_currentIndex));
    }

    private void LoadTutorial(TutorialContent tutorial)
    {

        _subtitle.text = tutorial.title;
        _content.text = tutorial.content;
        _imageA.sprite = tutorial.imageA;
        _imageB.sprite = tutorial.imageB;
    }
}
