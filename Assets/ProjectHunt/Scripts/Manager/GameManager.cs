using System.Collections;
using MadDuck.Scripts.Character;
using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Title("Game Status")]
    [SerializeField] private CharacterHub playerStatus;
    [SerializeField] private CharacterHub enemyStatus;
    
    [Title("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    
    [Title("Settings")]
    [SerializeField] private float waitAnimationTime = 1f;
    
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (playerStatus.ConditionState == CharacterConditionState.Dead)
        {
            StartCoroutine(UpdatePlayerStatus());
        }
        
        UpdateEnemyStatus();
    }
    
    IEnumerator UpdatePlayerStatus()
    {
        yield return new WaitForSeconds(waitAnimationTime);
        gameOverPanel.SetActive(true);
    }
    
    private void UpdateEnemyStatus()
    {
        if (enemyStatus.ConditionState == CharacterConditionState.Dead)
        {
            gameClearPanel.SetActive(true);
        }
    }
}
