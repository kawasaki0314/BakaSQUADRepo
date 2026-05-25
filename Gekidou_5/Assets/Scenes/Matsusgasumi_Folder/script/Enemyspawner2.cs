using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpeawner2: MonoBehaviour
{
    //ここ(関数の外)に書くことで、スクリプト内のどこからでも使えるようになります!
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] float SpawnInterVal = 2.0f; //  少し間隔を広げて2秒ごとに設定

    [SerializeField] int maxEnemyCount = 15;//画面に存在できる敵の最大数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //定期生成を行うコルーチン開始
        StartCoroutine(SpawnRoutine());
    }
//2. 足りなかったコルーチン本体です
private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(SpawnInterVal);

            //タグを使って、今画面にいる敵の数を数える
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            //もし最大数を超えていたら、この回はスキップする
            if(currentEnemyCount >= maxEnemyCount)
            {
                continue;
            }
        }
        /*
        //最大数に余裕がある分だけ、あるいは最大5匹生み出す
        for (int i = 0; i < 5; i++)

        {
            //生成する直前にもう一度上限チェック
            if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemyCount) break;

            SpawnEnemy();

        }
        */

    }
    //3. 足りなかった「実際に敵を生み出す処理」です
    private void SpawnEnemy()

    {

        if (EnemyPrefab == null) return; //これなら正常に見つかる!//半径10マスの円の中のランダムな位置を計算
        Vector2 randomOffset = Random.insideUnitCircle * 10f;

        Vector2 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);


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

