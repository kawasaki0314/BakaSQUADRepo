using Microsoft.Win32.SafeHandles;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timer = 0f;
    bool bossSpawned = false;
    [SerializeField]public GameObject BossPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 300f && !bossSpawned)
        {
            Instantiate(BossPrefab, Vector3.zero, Quaternion.identity);
            bossSpawned = true;
        }
    }
}
