using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
        //ゲーム開始時に通常の敵を初期数だけ生成する（これで画面に出るようになります！）
        for (int i = 0; i < initialSpawnCount; i++)
        {
            //広い範囲（例：-10から-10）を指定します
            float randomX = Random.Range(-10f, 10f);
            float randomY = Random.Range(-10f, 10f);

            Vector2 randomPos = new Vector2(randomX, randomY);

            SpawnspecificEnemy(false, randomPos);

        }
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
        //新しく出す場所を少しずらす
        Vector2 spawnPosition = defeatedPosition + Random.insideUnitCircle * 1.5f;

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