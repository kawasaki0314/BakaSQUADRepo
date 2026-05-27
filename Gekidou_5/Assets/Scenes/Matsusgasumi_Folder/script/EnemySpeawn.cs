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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //ゲーム開始時に通常の敵を初期数だけ生成する（これで画面に出るようになります！）
        for (int i = 0; i < initialSpawnCount; i++)
        {
            /*Vector2 randomOffset = Random.insideUnitCircle * 10f;
            Vector2 spawnPosition = (Vector2)transform.position + randomOffset;
            SpawnspecificEnemy(false, spawnPosition);//falseなので通常敵*/

            int spawnPosX = Random.Range(0, 4);
            int spawnPosY = Random.Range(0, 4);
            GameObject obj =Instantiate(regularEnemyprefab);
            obj.transform.position = new Vector2(spawnPosX, spawnPosY);


        }
    }
    //敵を生成する関数（引数でどっちの敵か指定する）
    private void SpawnspecificEnemy(bool isSpecial, Vector2 position)
    {

       if (isSpecial)
        {
            //特定のキャラの場合、上限に達していたら生成をキャンセルする
            if(currentSpecialCount >= maxSpecialEnemyCount)
            {
                Debug.Log("特定のキャラが上限に達しているため、生成をスキップしました。");
                return;
            }
            Instantiate(specialEnemyprefab, position, Quaternion.identity);
            currentSpecialCount++;//カウントアップ
        }
       else
        {
            Instantiate(specialEnemyprefab, position, Quaternion.identity);
            currentSpecialCount++;
        }
    }
    //敵が倒されたとき（敵側からよばれる）
    public void OnEnemyDefeated(bool isSpecial, Vector2 defeatedPosition)
    {
        if(isSpecial)
        {
            currentSpecialCount--;//特定のキャラが倒されたので数を減らす

            //倒されたので、また近くに同じ特定のキャラを1匹補充する（上限以下なので確実に生成される）
            Vector2 spawnPosition = defeatedPosition + Random.insideUnitCircle * 1f;
            SpawnspecificEnemy(true, spawnPosition);
        }
        else
        {
            currentRegularCount--;
            //通常の敵の補充処理...
        }
    }

}