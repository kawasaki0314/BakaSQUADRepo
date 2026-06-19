using Unity.VisualScripting;
using UnityEngine;
public class AIHoming : MonoBehaviour
{
    Transform playerTr;//プレイヤーのTransform
    [SerializeField] float speed = 2f;  //敵の動くスピード

    [Header("Enemy Status")]
    public int attackPower = 1;　//敵の攻撃力
    public float attackInterval = 1f;//攻撃のインターバル（1秒に1回）
    //public float attackTimer = 0f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // （既存のプレイヤーを探す処理はそのまま）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTr = playerObj.transform;

        // 【追加】敵自身のRigidbody2Dを取得しておく
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //物理移動は　Update　ではなく　FixedPdate で行うのがUnityの鉄則!
    private void Update()
    {
        //攻撃タイマーを常に進める（プレイヤーに触れる間だけカウントしたい場合は、下に 移動させてもOK)
        //attackTimer += Time.deltaTime;
    }

    //毎フレームのタイマー更新は　Update で行う
    private void FixedUpdate()
    {
        if (playerTr == null) return;

        // 【追加】衝突で発生した余計な吹っ飛び速度（慣性）をここで毎フレームリセットする
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // プレイヤーに向けて進む（元の処理のまま）
        transform.position = Vector2.MoveTowards(transform.position,
            playerTr.position,
            speed * Time.fixedDeltaTime);
    }

    //死亡処理
    public void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn なら、ここも EnemySpeawn に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");
        
        //画面内からプレイヤーのレベルスクリプトを探して、経験値を手渡す
        levelupplayer playerlevel = FindFirstObjectByType<levelupplayer>();
        if(playerlevel != null)
        {
            playerlevel.Addexperience(3);//敵を1体倒したら経験値「３」手に入れる設定
            Debug.Log("プレイヤーに経験値を３あたえました！");
        }
        else
        {
            //もしもこのエラーが出てたら、プレイヤーに「PlayerLevel」スクリプトが付いているか確認してください
            Debug.LogWarning("PlayerLevelスクリプトは見つかりません！経験値が加算さrませんでした。");
        }
        
        //Instance(シングルトン)を使ってスポナーに報告
        //EnemySpawnのInstance(さっきAwakeで作ったやつ)を直接呼ぶ
        if (EnemySpawn.Instance != null)
        {
            //倒された場所を伝えて補充してもらうう
            EnemySpawn.Instance.OnEnemyDefeated(false, transform.position);
           // Debug.Log("補充依頼しました");
        }
        else
        {
            //もしこれが出たら、Spawner側のAwakeが動いていない証拠です
            Debug.LogError("スポナーのInstanceが見つかりません！Spawnerオブジェクトにスクリプトを付け直してください");


        }
       
        //自分を消去する処理は、すべての報告や処理が「終わったら最後」に1回だけ書くのが鉄則！
        Destroy(gameObject);
    }
    
    
    //スポナーから一斉に消されるための関数
    public void Disapear()
    {
        Debug.Log("時間切れのため、一斉に消滅します。");

        //一斉に消える時は、個別に補充(OnEnemyDefeated)を呼ばす
        //そのまま自分自身を消去します(そうしないと消えた瞬間新しい敵が湧いてしまうため)
        Destroy(gameObject);
    }
}  