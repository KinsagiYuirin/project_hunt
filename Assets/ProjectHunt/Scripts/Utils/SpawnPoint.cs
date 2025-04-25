using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.transform.position = gameObject.transform.position;
    }

}
