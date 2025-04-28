using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerScript : MonoBehaviour
{
    [Title("Video Player")]
    [SerializeField] protected VideoPlayer cutScene;
    public VideoPlayer CutScene => cutScene;
    
    [SerializeField] protected RawImage videoImage;
    public RawImage VideoImage => videoImage;
    
    [SerializeField] protected RenderTexture renderTexture;
    public RenderTexture RenderTexture => renderTexture;
    
    protected virtual void Start()
    {
        ClearRenderTexture(renderTexture);
        
        cutScene.targetTexture = renderTexture;
        videoImage.texture = renderTexture;
        
        cutScene.Prepare();
        cutScene.prepareCompleted += OnVideoReady;
    }
    
    protected virtual void OnVideoReady(VideoPlayer vp)
    {
        vp.frame = 0;
        vp.Pause();
    }
    
    protected virtual void ClearRenderTexture(RenderTexture rt)
    {
        RenderTexture current = RenderTexture.active;
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = current;
    }
}
