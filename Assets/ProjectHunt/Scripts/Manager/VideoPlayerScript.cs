using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerScript : MonoBehaviour
{
    [Title("Video Player")]
    [SerializeField] private VideoPlayer cutScene;
    public VideoPlayer CutScene => cutScene;
    
    [SerializeField] private RawImage videoImage;
    public RawImage VideoImage => videoImage;
    
    [SerializeField] private RenderTexture renderTexture;
    public RenderTexture RenderTexture => renderTexture;
    
    void Start()
    {
        ClearRenderTexture(renderTexture);
        
        cutScene.targetTexture = renderTexture;
        videoImage.texture = renderTexture;
        
        cutScene.Prepare();
        cutScene.prepareCompleted += OnVideoReady;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnVideoReady(VideoPlayer vp)
    {
        vp.frame = 0;
        vp.Pause();
    }
    
    void ClearRenderTexture(RenderTexture rt)
    {
        RenderTexture current = RenderTexture.active;
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.black);  // เคลียร์ด้วยสีดำหรือใส่สีที่ต้องการ
        RenderTexture.active = current;
    }
}
