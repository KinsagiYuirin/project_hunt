using System;
using System.Collections;
using MadDuck.Scripts.Character;
using MadDuck.Scripts.Character.Module;
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
    
    [SerializeField] private CharacterHealthModule bossHealthModule;
    [SerializeField] private CharacterArmorModule bossArmorModule;
    [SerializeField] private BGMSystem[] bgmSystem;

    public void Initialize()
    {
        isStart = false;
    }
    
    public void Event(bool isBool, MonoBehaviour mono)
    {
        if (cameraZoom != null)
        {
            isStart = true;
            cameraZoom.SwitchCamera(!isBool);
            float targetPercent = Mathf.Clamp01(bossHealthModule.pHealthData.currentHealth / bossHealthModule.pHealthData.maxHealth);
            float targetArmorPercent = Mathf.Clamp01(bossArmorModule.PArmorData.currentArmor / bossArmorModule.PArmorData.maxArmor);
            mono.StartCoroutine(bossHealthModule.YuirinHealthBar.FillSmoothly(targetPercent));
            mono.StartCoroutine(bossArmorModule.YuirinHealthBar.FillSmoothly(targetArmorPercent));
            bgmSystem[0].StopBGM();
            bgmSystem[1].PlayBGM();
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
    [SerializeField] private CharacterMovementModule characterMovementModule;

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
        characterMovementModule.MoveDirection = Vector2.zero;
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

    [Header("Details")]
    [SerializeField] private bool haveLimit;
    [SerializeField, ShowIf("haveLimit")] private int useCount;
    
    private void Start()
    {
        cameraSetting?.Initialize();
        endGameSetting?.Initialize();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!switchActive) return;

        if (haveLimit)
        {
            if (useCount <= 0) switchActive = false;
            useCount--;
        }
        
        if (other.CompareTag("Player"))
        { ExecuteSwitch(); }
    }
    
    private void ExecuteSwitch()
    {
        if (!switchActive) return;
        switch (executeSwitchAction)
        {
            case ExecuteSwitchAction.Camera:
                cameraSetting.Event(turnOn, this);
                break;
            
            case ExecuteSwitchAction.EndGame:
                endGameSetting.Event(this);
                break;
        }
    }

}
