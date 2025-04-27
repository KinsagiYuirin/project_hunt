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
    [SerializeField] private GameObject gameClearPanel;

    [Title("Settings")]
    [SerializeField] private float prepareTime = 1f;
    [SerializeField] private float waitAnimationTime = 1f;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private RadialFader radialFader;
    [SerializeField] private CharacterMovementModule playerMovement;

    [Title("Switch")] 
    [SerializeField] private Switch switchPoint;
    
    [Title("Debug")]
    [SerializeField, DisplayAsString] private bool isPreparing = false;
    
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
        //radialFader.SetUpFadeMaterial();
        radialFader.StartRadialFadeOut(2f);
        
        enemyStatus.ChangeConditionState(CharacterConditionState.CutScene);
    }
    
    private void Update()
    {
        Debug.Log(playerStatus.ConditionState);
        
        if (switchPoint.IsStart && !isPreparing)
        {
            isPreparing = true;
            playerStatus.ChangeConditionState(CharacterConditionState.CutScene);
            StartCoroutine(PlayerPrepare());
        }
        
        if (playerStatus.ConditionState == CharacterConditionState.Dead)
        { StartCoroutine(UpdatePlayerStatus()); }
        
        if (enemyStatus.ConditionState == CharacterConditionState.Dead)
        { StartCoroutine(UpdateEnemyStatus());}
    }

    IEnumerator PlayerPrepare()
    {
        playerMovement.MoveDirection = Vector2.zero;
        yield return new WaitForSeconds(prepareTime);
        playerStatus.ChangeConditionState(CharacterConditionState.Normal);
        enemyStatus.ChangeConditionState(CharacterConditionState.Normal);
    }
        
    IEnumerator UpdatePlayerStatus()
    {
        if (cameraZoom == null) yield break;
        //cameraZoom.ZoomActive = false;
        cameraZoom.SwitchCamera(false);
        yield return new WaitForSeconds(waitAnimationTime);
        gameOverPanel.SetActive(true);
    }
    
    IEnumerator UpdateEnemyStatus()
    {
        if (cameraZoom == null) yield break;
        //cameraZoom.ZoomActive = false;
        cameraZoom.SwitchCamera(false);
        yield return new WaitForSeconds(waitAnimationTime);
        gameClearPanel.SetActive(true);
    }
}
