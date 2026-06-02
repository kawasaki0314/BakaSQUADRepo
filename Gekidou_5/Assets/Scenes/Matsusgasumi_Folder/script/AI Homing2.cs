using Unity.VisualScripting;
using UnityEngine;
public class AIHoming2 : MonoBehaviour
{
    Transform playerTr;//プレイヤーのTransform
    [SerializeField] float speed = 5f;  //敵の動くスピード

    [Header("Enemy Status")]
    [SerializeField] int maxHP = 3; //敵の最大HP
    [SerializeField] int attackPower = 1;　//敵の攻撃力
    [SerializeField] float attackInterval = 1f;//攻撃のインターバル（1秒に1回）
    float attackTimer = 0f;

    int currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //現在のHPを最大HPと同じ値に初期化します。
        currentHP = maxHP;
        FindPlayer();
        
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
        //もしプレイヤーが見つからなかっていなければ、その場で探す
        if(playerTr == null)
        {
            FindPlayer();
        }

        // それでも見つからなければ、ここで処理を中断して次のフレームを待つ
        if (playerTr == null) return;

        //プレイヤーに向けて移動する
        //speedが小さすぎると動いて見えないので、インスペクターで「5」くらいにしてみてください
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTr.position,
            speed * Time.fixedDeltaTime);
    }


    //ダメージを受ける関数(ここがHP0で消滅するコアの部分です)
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("敵の残りHP: " + currentHP);

        //HPが0以下なら死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }

    //死亡処理
    void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn2 なら、ここも EnemySpeawn2 に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");
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
    //プレイヤーに当たった瞬間（最初の1発）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに接触！");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackPower);
                attackTimer = 0f;//接触週刊にタイマーリセット
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Upbateでも進めていますが、念のためここにもチャック
            if(attackTimer >= attackInterval)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackPower);
                    Debug.Log("継続ダメージを与えました！");
                    attackTimer = 0f;
                }
            }
        }
    }

    //プレイヤーが離れたらタイマーをリセット（スペルを修正しました）
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが離れました");
            attackTimer = 0f;//離れたらリセット
        }
    }
}
