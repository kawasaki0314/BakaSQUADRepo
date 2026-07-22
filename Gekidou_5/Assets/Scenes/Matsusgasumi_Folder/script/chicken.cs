using UnityEngine;

public class chicken : MonoBehaviour
{
    private Vector2 pos;
    public int num = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        //マイナスをかけることで逆方向にいどうする
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if(pos.x > -2)
        {
            num = -1;
        }
        if(pos.x < -11)
        {
            num = 1;
        }
    }

    //死亡処理
    public void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn なら、ここも EnemySpeawn に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");

        //画面内からプレイヤーのレベルスクリプトを探して、経験値を手渡す
        levelupplayer playerlevel = FindFirstObjectByType<levelupplayer>();
        if (playerlevel != null)
        {
            playerlevel.Addexperience(1500);//敵を1体倒したら経験値「1500」手に入れる設定
            Debug.Log("プレイヤーに経験値を1500あたえました！");
        }
        else
        {
            //もしもこのエラーが出てたら、プレイヤーに「PlayerLevel」スクリプトが付いているか確認してください
            Debug.LogWarning("PlayerLevelスクリプトは見つかりません！経験値が加算さrませんでした。");
        }

        //Instance(シングルトン)を使ってスポナーに報告
        //EnemySpawnのInstance(さっきAwakeで作ったやつ)を直接呼ぶ
        if (spawn.Instance != null)
        {
            //倒された場所を伝えて補充してもらうう
            spawn.Instance.OnEnemyDefeated(false, transform.position);
            // Debug.Log("補充依頼しました");
        }
        else
        {
            //もしこれが出たら、Spawner側のAwakeが動いていない証拠です
            Debug.LogError("spawnのInstanceが見つかりません！Spawnerオブジェクトにスクリプトを付け直してください");
        }

        //じぶんおｗ消去する処理は、すべての報告や処理が「終わったら最後」に1回だけ書くのが鉄則！
        Destroy(gameObject);
    }

}
