using System.Collections;
using System.Numerics;
using MadDuck.Scripts.Character;
using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class GameManager : MonoBehaviour
{
    [Title("Game Status")]
    [SerializeField] private CharacterHub playerStatus;
    [SerializeField] private CharacterHub enemyStatus;
    
    [Title("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    private FadeUI _gameOverFadeUI;
    [SerializeField] private GameObject gameClearPanel;
    private FadeUI _gameClearFadeUI;
    
    [Title("Settings")]
    [SerializeField] private float prepareTime = 1f;
    [SerializeField] private float waitAnimationTime = 1f;
    [SerializeField] private float waitBossAnimetionTime = 5f;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private RadialFader radialFader;
    [SerializeField] private CharacterMovementModule playerMovement;
    [SerializeField] private ParticleSystem darkLightParticle;
    
    [Title("BGM")]
    [SerializeField] private BGMSystem[] bgmSystem;
    
    [Title("Game Cut Scene Settings")]
    [SerializeField] private FadeSlideUI fadeSlideUI;
    [SerializeField] private bool haveCutScene;
    [SerializeField, ShowIf("haveCutScene")] private InGameCutSceneManager inGameCutScene;

    [Title("Switch")] 
    [SerializeField] private Switch[] switchPoint;
    
    [Title("Debug")]
    [SerializeField, DisplayAsString] private bool isPreparing = false;
    [SerializeField, DisplayAsString] private bool isCutSceneOn = false;
    [SerializeField, DisplayAsString] private bool isCoroutineRunning;
    
    public bool IsCutSceneOn => isCutSceneOn;
    
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        
        bgmSystem[0].PlayBGM();
        
        darkLightParticle.Stop();
        
        _gameOverFadeUI = gameOverPanel.GetComponent<FadeUI>();
        _gameClearFadeUI = gameClearPanel.GetComponent<FadeUI>();
        
        radialFader.StartRadialFadeOut(2f);

        fadeSlideUI.StayDuration = (float)inGameCutScene.VideoLength;
        
        enemyStatus.ChangeConditionState(CharacterConditionState.CutScene);
    }
    
    private void Update()
    {
        Debug.Log(enemyStatus.ConditionState);
        
        if (ShouldPreparePlayer())
        {
            isPreparing = true;
            playerStatus.ChangeConditionState(CharacterConditionState.CutScene);
            StartCoroutine(PlayerPrepare());
        }
        
        if (playerStatus.ConditionState == CharacterConditionState.Dead && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(UpdatePlayerStatus());
        }
        
        if (enemyStatus.ConditionState == CharacterConditionState.Dead && !isCutSceneOn)
        {
            TriggerCutScene();
            StartCoroutine(UpdateEnemyStatus());
        }
    }
    
    private bool ShouldPreparePlayer()
    { return switchPoint[0].CameraSetting.IsStart && !isPreparing; }
    
    private void TriggerCutScene()
    {
        isCutSceneOn = true;
        switchPoint[1].switchActive = true;
    }

    private IEnumerator PlayerPrepare()
    {
        playerMovement.MoveDirection = Vector2.zero;
        yield return new WaitForSeconds(prepareTime);
        playerStatus.ChangeConditionState(CharacterConditionState.Normal);
        enemyStatus.ChangeConditionState(CharacterConditionState.Normal);
    }

    private IEnumerator UpdatePlayerStatus()
    {
        if (cameraZoom == null) yield break;
        playerMovement.MoveDirection = Vector2.zero;
        enemyStatus.ChangeConditionState(CharacterConditionState.CutScene);
        
        //cameraZoom.ZoomActive = false;
        cameraZoom.SwitchCamera(false);
        yield return new WaitForSeconds(waitAnimationTime);
        
        radialFader.StartRadialFadeIn(1f);
        StartCoroutine(_gameOverFadeUI.FadeIn());
        enemyStatus.ChangeConditionState(CharacterConditionState.Normal);
    }

    private IEnumerator UpdateEnemyStatus()
    {
        if (cameraZoom == null) yield break;

        playerStatus.ChangeConditionState(CharacterConditionState.CutScene);
        enemyStatus.ChangeConditionState(CharacterConditionState.CutScene);
        cameraZoom.SwitchCamera(false);

        if (haveCutScene)
        {
            yield return StartCoroutine(inGameCutScene.PlayCutscene());
        }
        
        yield return new WaitForSeconds(waitBossAnimetionTime);
        enemyStatus.ChangeConditionState(CharacterConditionState.Dead);
        
        bgmSystem[1].StopBGM();
        bgmSystem[0].PlayBGM();
        darkLightParticle.Play();
        playerStatus.ChangeConditionState(CharacterConditionState.Normal);
    }
    
}
