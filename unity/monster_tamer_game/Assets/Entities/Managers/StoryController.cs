using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TypewriterText textWindow;

    [Header("Story Configuration")]
    [SerializeField] private List<string> storyPoints;
    [SerializeField] private string nextSceneName = "Battle";

    
    [Header("Debug Info")]
    [SerializeField] private int currentStoryIndex;

    void Awake()
    {
        AdvanceStory(first: true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
            AdvanceStory();
    }

    private void AdvanceStory(bool first = false)
    {
        if (currentStoryIndex >= storyPoints.Count && !textWindow.IsTyping)
        {
            LoadNextScene();
            return;
        }

        if (textWindow.IsTyping) textWindow.CompleteTyping();
        else textWindow.SetText(storyPoints.ElementAt(currentStoryIndex++));
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);

    }

}
