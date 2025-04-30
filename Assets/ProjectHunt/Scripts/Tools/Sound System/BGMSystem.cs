using System.Collections;
using TriInspector;
using UnityEngine;

public class BGMSystem : MonoBehaviour
{
    [Title("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float MaxVolume = 1f;

    private void Awake()
    {
        bgmAudioSource.volume = MaxVolume;
    }
    
    public void PlayBGM()
    {
        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.Play();
        FadeIn();
    }

    public void StopBGM()
    {
        FadeOut();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAudio(bgmAudioSource, MaxVolume, 0f));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeAudio(bgmAudioSource, 0f, MaxVolume));
    }

    private IEnumerator FadeAudio(AudioSource source, float startVolume, float endVolume)
    {
        float elapsed = 0f;
        source.volume = startVolume;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / fadeDuration);
            yield return null;
        }

        source.volume = endVolume;

        if (endVolume == 0f)
        {
            source.Stop();
        }
    }
}