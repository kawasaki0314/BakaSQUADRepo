using UnityEngine;
using System.Collections;


public class EnemySpawn2 : MonoBehaviour
{
    public static EnemySpawn2 Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] GameObject regularEnemyprefab;//通常の敵
    [SerializeField] GameObject specialEnemyprefab;//制限したい特徴のキャラ

    [Header("Spawn Limits")]
    [SerializeField] int initialSpawnCount = 5;
    [SerializeField] int maxSpecialEnemyCount = 8;//このキャラは画面最大20匹まで

    [Header("Timer Settings")]
    [SerializeField] float delaySeconds = 30f; // 何秒後に登場させるか（インスペクターで変更可能）

    private int currentRegularCount = 0;
    private int currentSpecialCount = 0;//このキャラの現在の数

    //プレイヤーの場所を基準にするための変数
    private Transform playerTransform;

    //画面外から出現させるための距離設定
    [Header("Spawn Distance")]
    [SerializeField] float misSpawnDistance = 12f;  //最低でもこのくらい離す（画面外）
    [SerializeField] float maxSpawnDistance = 15f;  //最大でもこのくらいの距離（遠すぎない）

    [Header("Global Wave Limits")]
    [SerializeField] float waveTimeLimit = 300f; //全員が消えるまでの制限時間（例: 30秒)
    private float waveTimer = 0;
    private bool waveEnded = false;  //二重に消滅処理を防ぐフラグ 
    private bool isspawningStarted = false;//タイマーが終了して生成が始まったかどうかのフラグ

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        //プレイヤーをタグで見つける
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("EnemySpawn2: タグ'Player'が見つかりません！");
        }

        // 直接生成せず、タイマー（コルーチン）をスタートさせる
        StartCoroutine(SpawnAfterDelay());
    }

    // 時間を待ってから生成する処理
    private IEnumerator SpawnAfterDelay()
    {
        // 指定した秒数だけ待機
        yield return new WaitForSeconds(delaySeconds);

        // 待機が終わったので、プレイヤーの画面外に初期数だけ生成
        for (int i = 0; i < initialSpawnCount; i++)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            SpawnspecificEnemy2(false, spawnPos);
        }

        isspawningStarted = true;

        Debug.Log($"{delaySeconds}秒経過したので敵を生成しました！");
    }

    //プレイヤーの周囲（画面外）のランダムな位置を計算する関数
    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 basePosition = Vector2.zero;
        if(playerTransform != null)
        {
            basePosition = playerTransform.position;
        }

        //ランダムな方向（角度）を決める
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        //画面外になるようなランダムな距離を決める
        float distance = Random.Range(misSpawnDistance, maxSpawnDistance);

        //方向と距離から、位置(X, Y)を計算
        float spawnX = basePosition.x + Mathf.Cos(angle) * distance;
        float spawnY = basePosition.y + Mathf.Sin(angle) * distance;

        return new Vector2(spawnX, spawnY);
    }
    // --- 以下、SpawnspecificEnemy と OnEnemyDefeated は元のまま ---
    private void SpawnspecificEnemy2(bool isSpecial, Vector2 position)
    {
        GameObject prefabToSpawn = isSpecial ? specialEnemyprefab : regularEnemyprefab;
        if (prefabToSpawn == null)
        {
            Debug.LogError("プレハブが設定されてません！インスペクターを確認してください。");
            return;
        }
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
    //敵が死んだときに「敵自身から」呼ばれる関数
    public void OnEnemyDefeated(bool isSpecial, Vector2 defeatedPosition)
    {
        //ウェーブがすでに終了しているなら補充しない
        if (waveEnded) return;

        //敵が死んだときの補充も、現在のプレイヤーの画面外にする
        Vector2 spawnPosition = GetRandomSpawnPosition();
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
        Debug.Log($"敵が倒されたので補充しました。通常:{currentRegularCount}特集:{currentSpecialCount}");
    }

    private void Update()
    {
        if (waveEnded) return;//すべて終わっていれば何もしない
        if (!isspawningStarted) return;//敵がまだ出現していない（30秒待っている間）ならタイマーを進めない

        waveTimer += Time.deltaTime;

        if (waveTimer >= waveTimeLimit)
        {
            EndWaveAndClearEnemies();
        }
    }

    //時間が過ぎたらすべての敵を消去する関数
    private void EndWaveAndClearEnemies()
    {
        waveEnded = true;
        Debug.Log("制限時間になりました！すべての敵を消去します。");
        
        //画面内にいるすべての「AIHoming2」スクリプトが付いたオブジェクトを探してリストする
        AIHoming2[] allEnemies = FindObjectsByType<AIHoming2>(FindObjectsSortMode.None);
        
        //ループ処理で、見つかった敵すべてに「Disapppear()」を実行させる
        foreach (AIHoming2 enemy in allEnemies)
        {
            if(enemy != null)
            {
                enemy.Disapear();//AIHoming2側の消滅エフェクトなどを実行
            }
        }
        
        //カウントをリセット
        currentRegularCount = 0;
        currentSpecialCount = 0;

        Debug.Log("すべての敵が消去が完了しました。");
    }
}
