using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class EnemySpawn2 : MonoBehaviour
{
    public static EnemySpawn2 Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] GameObject regularEnemyprefab;
    [SerializeField] GameObject specialEnemyprefab;

    [Header("Spawn Limits")]
    [SerializeField] int initialSpawnCount = 15;
    [SerializeField] int maxSpecialEnemyCount = 20;

    [Header("Timer Settings")]
    [SerializeField] float delaySeconds = 3f; // 何秒後に登場させるか（インスペクターで変更可能）

    private int currentRegularCount = 0;
    private int currentSpecialCount = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // 直接生成せず、タイマー（コルーチン）をスタートさせる
        StartCoroutine(SpawnAfterDelay());
    }

    // 時間を待ってから生成する処理
    private IEnumerator SpawnAfterDelay()
    {
        // 指定した秒数だけ待機
        yield return new WaitForSeconds(3f);

        // 待機が終わった後に、元の生成処理を実行
        for (int i = 0; i < initialSpawnCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomY = Random.Range(-10f, 10f);
            Vector2 randomPos = new Vector2(randomX, randomY);

            SpawnspecificEnemy2(false, randomPos);
        }

        Debug.Log($"{delaySeconds}秒経過したので敵を生成しました！");
    }

    // --- 以下、SpawnspecificEnemy と OnEnemyDefeated は元のまま ---
    private void SpawnspecificEnemy2(bool isSpecial, Vector2 position)
    {
        GameObject prefabToSpawn = isSpecial ? specialEnemyprefab : regularEnemyprefab;
        if (prefabToSpawn == null) return;

        if (isSpecial)
        {
            if (currentSpecialCount >= maxSpecialEnemyCount) return;
            Instantiate(specialEnemyprefab, position, Quaternion.identity);
            currentSpecialCount++;
        }
        else
        {
            Instantiate(regularEnemyprefab, position, Quaternion.identity);
            currentRegularCount++;
        }
    }

    public void OnEnemyDefeated(bool isSpecial, Vector2 defeatedPosition)
    {
        Vector2 spawnPosition = defeatedPosition + Random.insideUnitCircle * 10f;
        if (isSpecial)
        {
            currentSpecialCount--;
            SpawnspecificEnemy2(true, spawnPosition);
        }
        else
        {
            currentRegularCount--;
            SpawnspecificEnemy2(false, spawnPosition);
        }
    }
}
