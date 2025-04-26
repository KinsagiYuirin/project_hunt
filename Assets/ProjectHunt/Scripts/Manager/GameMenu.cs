using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameMenu : MonoBehaviour
{
    [Title("Setting")]
    [SerializeField] private FadeUI[] fadeUI;
    [SerializeField] private RadialFader radialFader;

    public void RestartButton()
    {
        radialFader.StartRadialFadeIn(0f);
        StartCoroutine(ButtonSet1(1));
    }
    
    public void MainMenuButton()
    {
        radialFader.StartRadialFadeIn(0f);
        StartCoroutine(ButtonSet1(2));
    }
    
    IEnumerator ButtonSet1(int buttonIndex)
    {
        yield return StartCoroutine(fadeUI[0].FadeOut());

        switch (buttonIndex)
        {
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 2:
                SceneManager.LoadScene("MainMenu");
                 break;
        }
    }

    public void ContinueButton()
    {
        radialFader.StartRadialFadeIn(0f);
        StartCoroutine(ContinueIE());
    }
    
    IEnumerator ContinueIE()
    {
        yield return StartCoroutine(fadeUI[2].FadeOut());
        SceneManager.LoadScene("EndCredit");
    }
}
