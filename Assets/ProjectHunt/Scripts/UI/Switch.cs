using System;
using MadDuck.Scripts.Character;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum ExecuteSwitchAction
{
    Camera,
    EndGame
}

[Serializable]
public class CameraSetting
{
    public CameraZoom cameraZoom;
    [SerializeField] private bool isStart;
    public bool IsStart { get => isStart; set => isStart = value; }

    public void Initialize()
    {
        isStart = false;
    }
    
    public void Event(bool isBool)
    {
        if (cameraZoom != null)
        {
            isStart = true;
            cameraZoom.SwitchCamera(!isBool);
        }
        else { Debug.LogWarning("CameraZoom is not assigned in CameraSetting!"); }
    }
}
 
[Serializable]
public class EndGameSetting
{
    [SerializeField] private CharacterHub playerStatus;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private FadeUI clearPanelFadeUI;

    public void Initialize()
    {
        
    }

    public void Event(MonoBehaviour mono)
    {
        if (clearPanel == null)
        {
            Debug.LogWarning("ClearPanel is not assigned in EndGameSetting!");
            return;
        }
        clearPanel.SetActive(true);
        playerStatus.ChangeConditionState(CharacterConditionState.CutScene);
        mono.StartCoroutine(clearPanelFadeUI.FadeIn());
    }
}

public class Switch : MonoBehaviour
{
    [Title("Switch Settings")]
    public bool switchActive = false;
    [SerializeField] private ExecuteSwitchAction executeSwitchAction;
    
    // isOff ต้องการให้สวิตช์เป็น ปิด หรือ เปิด
    // เริมต้นจะเป็น เปิด
    [SerializeField] private bool turnOn;
    
    [SerializeField, ShowIf(nameof(executeSwitchAction), ExecuteSwitchAction.Camera)] 
    private CameraSetting cameraSetting;
    public CameraSetting CameraSetting { get => cameraSetting; set => cameraSetting = value; }
    
    [SerializeField, ShowIf(nameof(executeSwitchAction), ExecuteSwitchAction.EndGame)] 
    private EndGameSetting endGameSetting;
    public EndGameSetting EndGameSetting { get => endGameSetting; set => endGameSetting = value; }

    private void Start()
    {
        cameraSetting?.Initialize();
        endGameSetting?.Initialize();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!switchActive) return;
        if (other.CompareTag("Player"))
        { ExecuteSwitch(); }
    }
    
    private void ExecuteSwitch()
    {
        if (!switchActive) return;
        switch (executeSwitchAction)
        {
            case ExecuteSwitchAction.Camera:
                cameraSetting.Event(turnOn);
                break;
            
            case ExecuteSwitchAction.EndGame:
                endGameSetting.Event(this);
                break;
        }
    }

}
