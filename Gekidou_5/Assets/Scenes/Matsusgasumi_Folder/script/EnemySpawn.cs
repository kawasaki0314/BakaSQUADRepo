using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySpawn : MonoBehaviour
{
    //他クラスから呼べるようにシングルトン(Instance)を有効化
    public static EnemySpawn Instance { get; private set; }

    //【追加】これがないとAIHoming側から「Enemyspawner.Instance」で呼べません
    //ここ(関数の外)に書くことで、スクリプト内のどこからでも使えるようになります!
    [Header("Prefabs")]
    [SerializeField] GameObject regularEnemyprefab;//通常の敵
    [SerializeField] GameObject specialEnemyprefab;//制限したい特定のキャラ

    [Header("Spawn Limits")]
    // 最初に出す通常の敵の数
    [SerializeField] int initialSpawnCount = 15;
    [SerializeField] int maxSpecialEnemyCount = 20;//このキャラは画面に最大20匹まで

    //各キャラクターの現在の出現数を数える変数
    private int currentRegularCount = 0;
    private int currentSpecialCount = 0;//このキャラの現在の数

    //プレイヤーの場所を基準にするための変数
    private Transform playerTransform;

    //画面外から出現させるための距離設定
    [Header("Spawn Distance")]
    [SerializeField] float misSpawnDistance = 12f;  //最低でもこのくらい離す（画面外）
    [SerializeField] float maxSpawnDistance = 15f;  //最大でもこのくらいの距離（遠すぎない）
    private void Awake()
    {
        //シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
            //シーンを跨がない場合は DontDestroyOnLoad は不要です
        }
        else
        {
            Destroy(gameObject);//重複を防ぐ
        }
    }

    private void Start()
    {
        //プレイヤーにタグで見つける
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("enemySpawn: タグ'Player'が見つかりません!");
        }

        //ゲーム開始時に通常の敵を初期化数だけ生成する
        for(int i = 0; i < initialSpawnCount; i++)
        {
            //最初からプレイヤーの画面外に配置する
            Vector2 spawnPos = GetRandomSpawnPosition();
            SpawnspecificEnemy(false, spawnPos);
        }
    }

    //プレイヤーの周囲（画面外）のランダムな位置を計算する関数
    private Vector2 GetRandomSpawnPosition()
    {
        //もしプレイヤーが見つかっていない場合は、原点（0,0)を基準にする
        Vector2 basePosition = Vector2.zero;
        if(playerTransform != null)
        {
            basePosition = playerTransform.position;
        }
        //ランダムな方向（角度）を決める
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        //画面外になるようなランダムな距離を決める
        float distance = Random.Range(misSpawnDistance, maxSpawnDistance);

        //方向と距離から、位置（X, Y)を計算
        float spawnX = basePosition.x + Mathf.Cos(angle) * distance;
        float spawnY = basePosition.y + Mathf.Sin(angle) * distance;

        return new Vector2(spawnX, spawnY);
    }
    //敵を生成する関数（引数でどっちの敵か指定する）
    private void SpawnspecificEnemy(bool isSpecial, Vector2 position)
    {
        GameObject prefabToSpawn = isSpecial ? specialEnemyprefab : regularEnemyprefab;

        // もしプレハブが空っぽ（消えている）なら、処理を中断する
        if (prefabToSpawn == null)
        {
            Debug.LogError("プレハブが設定されていません！インスペクターを確認してください。");
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
        //敵が死んだときの補充も、現在のプレイヤーの画面外にする
        Vector2 spawnPosition = GetRandomSpawnPosition();

        if(isSpecial)
        {
            currentSpecialCount--;
            SpawnspecificEnemy(true, spawnPosition);
        }
        else
        {
            currentRegularCount--;
            //通常敵が倒されたときも補充する
            SpawnspecificEnemy(false, spawnPosition);
        }

        Debug.Log($"敵が倒されたので補充しました。通常:{currentRegularCount}特殊：{currentSpecialCount}");
    }    
        
        
    

}