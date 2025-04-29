using System;
using System.Collections;
using TriInspector;
using UnityEngine;

public class InGameCutSceneManager : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private VideoPlayerScript videoPlayerScript;
    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private float timeScaleDuration = 1f;
    [SerializeField] private FadeSlideUI fadeSlideUI;

    [Title("Debug")] 
    [SerializeField, DisplayAsString] private double videoLength;
    public double VideoLength { get => videoLength;}

    private void Awake()
    {
        videoLength = videoPlayerScript.CutScene.clip.length;
    }

    public void StartCutsceneFlow()
    {
        StartCoroutine(SlowDownAndPlayCutscene());
    }

    public IEnumerator SlowDownAndPlayCutscene()
    {
        // ค่อยๆลดความเร็ว
        yield return StartCoroutine(ChangeTimeScale(1f, 0f, timeScaleDuration));

        // พอถึง 0 แล้ว เล่น Cutscene
        yield return StartCoroutine(PlayCutscene());

        // หลัง Cutscene จบ ค่อยๆเพิ่มความเร็ว
        yield return StartCoroutine(ChangeTimeScale(0f, 1f, timeScaleDuration));
    }

    private IEnumerator ChangeTimeScale(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        Time.timeScale = to;
    }

    public IEnumerator PlayCutscene()
    {
        Debug.Log("Cutscene Start");
        fadeSlideUI.StartFadeSlide();
        videoPlayerScript.CutScene.Play();

        yield return new WaitForSecondsRealtime((float)videoLength); 

        Debug.Log("Cutscene End");
    }
}
