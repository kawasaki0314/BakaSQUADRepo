using Unity.VisualScripting;
using UnityEngine;
public class AIHoming2 : MonoBehaviour
{
    Transform playerTr;//プレイヤーのTransform
    [SerializeField] float speed = 4f;  //敵の動くスピード

    [Header("Enemy Status")]
    public int attackPower = 1;　//敵の攻撃力
    public float attackInterval = 1f;//攻撃のインターバル（1秒に1回）
    public float attackTimer = 0f;

    int currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        FindPlayer();//生まれた瞬間に一度探す
        
    }

    //プレイヤーを探す処理を共通化
    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
           playerTr = playerObj.transform;
        }
    }
    // Update is called once per frame
    //物理移動は　Update　ではなく　FixedPdate で行うのがUnityの鉄則!
    private void Update()
    {
        //タイマーは常に進める
        attackTimer += Time.deltaTime;
    }

    //毎フレームのタイマー更新は　Update で行う
    private void FixedUpdate()
    {
        //プレイヤーが見つかっていない、または見失った場合は「毎回」探す
        if(playerTr == null)
        {
            FindPlayer();
        }
        //それでも見つからない時だけ処理をスキップする
        if (playerTr == null) return;

        //プレイヤーに向けて移動する（time.fixedDeltaTimeを使用）
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTr.position,
            speed * Time.fixedDeltaTime);
    }

    //死亡処理
    public void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn2 なら、ここも EnemySpeawn2 に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");
        
        //画面内からプレイヤーのレベルスクリプトを探して、経験値を手渡す
        PlayerLevel playerlevel = FindFirstObjectByType<PlayerLevel>();
        if (playerlevel != null)
        {
            playerlevel.GainExp(3);//敵を1体倒したら経験値「３」手に入れる設定
            Debug.Log("プレイヤーに経験値を３あたえました！");
        }
        else
        {
            //もしもこのエラーが出てたら、プレイヤーに「PlayerLevel」スクリプトが付いているか確認してください
            Debug.LogWarning("PlayerLevelスクリプトは見つかりません！経験値が加算さrませんでした。");
        }
        
        // 【修正】まず最初に、確実に自分を消す予約を入れる
        // Destroyは関数の最後に実行されるので、上に書いても大丈夫です

        //Instance(シングルトン)を使ってスポナーに報告
        //EnemySpawnのInstance(さっきAwakeで作ったやつ)を直接呼ぶ
        if (EnemySpawn2.Instance != null)
        {
            //倒された場所を伝えて補充してもらうう
            EnemySpawn2.Instance.OnEnemyDefeated(false, transform.position);
            // Debug.Log("補充依頼しました");
        }
        else
        {
            //もしこれが出たら、Spawner側のAwakeが動いていない証拠です
            Debug.LogError("Enemy2のInstanceが見つかりません！Spawnerオブジェクトにスクリプトを付け直してください");

        }

        Destroy(gameObject);
    }
    
    //スポナーから一斉に消されるための関数
    public void Disapear()
    {
        Debug.Log("時間が切れたため、一斉に消滅します。");

        //一斉に消える時は、個別に補充(OnEnemyDefeated)を呼ばず
        //そのまま自分自身を消去します（そうしないと消えた瞬間新しい敵が湧いてしまうため）
        Destroy(gameObject);
    }
}
