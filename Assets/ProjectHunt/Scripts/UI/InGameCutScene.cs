using TriInspector;
using UnityEngine;

public class InGameCutScene : VideoPlayerScript
{
    [SerializeField] private VideoPlayerScript videoPlayerScript;
    [SerializeField] private GameObject videoPlayer;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    private void StartCutScene()
    {
        videoPlayerScript.CutScene.Play();
    }
}
