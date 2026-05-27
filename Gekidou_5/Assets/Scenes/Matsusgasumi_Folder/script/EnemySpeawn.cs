using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemyspawner : MonoBehaviour
{
    //【追加】これがないとAIHoming側から「Enemyspawner.Instance」で呼べません
    //   public static Enemyspawner Instance { get; private set; }
    //ここ(関数の外)に書くことで、スクリプト内のどこからでも使えるようになります!
    [SerializeField] GameObject EnemyPrefab;
    [Header("Initial Settings")]
    [SerializeField] int initialSpawnCount = 15;//最初に何匹だしておくか
    //[SerializeField] float SpawnInterVal = 2.0f; //  少し間隔を広げて2秒ごとに設定

    
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //ゲーム開始時に、指定した数だけ敵を生み出す
        for (int i = 0; i < initialSpawnCount; i ++)
        {
            SpawnEnemy();
        }
    }

    

    //ランダムな位置に敵を１匹生成する
    private void SpawnEnemy()
    {

       if (EnemyPrefab == null) return; //これなら正常に見つかる!//半径10マスの円の中のランダムな位置を計算
        Vector2 randomOffset = Random.insideUnitCircle * 10f;
        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;
        //計算したランダム位置に複製
        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
    }
    //敵が倒されたときに、敵から呼ばれる窓口関数
    public void OnEnemyDefeated(Vector2 defeatedPosition)
    {
        //倒された場所の「ちょっとだけランダムにずらした位置」を計算
        Vector2 randomOffset = Random.insideUnitCircle * 1f;//1マス以内のズレ
        Vector2 spawnPosition = defeatedPosition + randomOffset;

        //敵が倒されたその場所に、新しく複製する！
        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("【テスト】敵の近くに新しく複製しました！");
    }

}