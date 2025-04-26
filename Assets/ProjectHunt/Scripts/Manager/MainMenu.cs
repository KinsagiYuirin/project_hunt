using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Title("Settings")]
    private AsyncOperation asyncLoad;
    [SerializeField] private VideoPlayerScript videoPlayer;
    [SerializeField] private RadialFader fader;
    [SerializeField] private GameObject buttonPanel;

    private void Start()
    {
        fader.NeedRevert = false;
        //fader.SetUpFadeMaterial();
        
        videoPlayer.CutScene.targetTexture = videoPlayer.RenderTexture;
        videoPlayer.VideoImage.texture = videoPlayer.RenderTexture;
        videoPlayer.CutScene.loopPointReached += ActivateScene;
    }
    
    public void StartButton()
    {
        StartCoroutine(PreloadScene("GameScene"));
        videoPlayer.CutScene.Play();
        buttonPanel.SetActive(false);
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }

    IEnumerator PreloadScene(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // รอจนกว่าจะโหลดถึง 90% (Unity จะหยุดไว้ที่ประมาณ 0.9)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }

        Debug.Log("Scene ready! Waiting for activation...");
    }

    private void ActivateScene(VideoPlayer vp)
    {
        StartCoroutine(WaitForFadeThenActivate());
        videoPlayer.CutScene.loopPointReached -= ActivateScene;
    }
    
    private IEnumerator WaitForFadeThenActivate()
    {
        fader.StartRadialFadeIn(0);
        
        yield return new WaitUntil(() => fader.IsFadeFinsh);

        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}