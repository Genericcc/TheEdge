using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument menuDocument;
    private Button playButton;
    private Button settingsButton;
    private Button exitButton;

    private void Awake() 
    {
        menuDocument = GetComponent<UIDocument>();

        playButton = menuDocument.rootVisualElement.Q<Button>("PlayButton");
        playButton.clicked += PlayButton_OnClicked;

        settingsButton = menuDocument.rootVisualElement.Q<Button>("SettingsButton");
        settingsButton.clicked += SettingsButton_OnClicked;

        exitButton = menuDocument.rootVisualElement.Q<Button>("ExitButton");
        exitButton.clicked += ExitButton_OnClicked;
    }

    private void PlayButton_OnClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void SettingsButton_OnClicked()
    {
        
    }

    private void ExitButton_OnClicked()
    {
        Application.Quit();
    }

}