using Microsoft.Win32.SafeHandles;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timer = 0f;
    Transform playerTransform;
    bool bossSpawned = false;
    public GameObject BossPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 60f && !bossSpawned)
        {
            Instantiate(BossPrefab, Vector3.zero, Quaternion.identity);
            bossSpawned = true;
        }
    }
}
