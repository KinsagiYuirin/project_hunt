using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    [SerializeField] private VideoPlayer videoPlayer;

    public void StartButton()
    {
        StartCoroutine(PreloadScene("GameScene"));
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

    // เรียกตอนที่ต้องการเข้าสู่ฉากใหม่
    public void ActivateScene()
    {
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}