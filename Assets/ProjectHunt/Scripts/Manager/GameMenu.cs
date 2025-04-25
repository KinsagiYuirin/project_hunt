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

    public void RestartButton()
    {
        StartCoroutine(RestartIE());
    }

    IEnumerator RestartIE()
    {
        yield return StartCoroutine(fadeUI[0].FadeOut());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void MainMenuButton()
    {
        StartCoroutine(MainMenrIE());
    }
    
    IEnumerator MainMenrIE()
    {
        yield return StartCoroutine(fadeUI[1].FadeOut());
        SceneManager.LoadScene("MainMenu");
    }

}
