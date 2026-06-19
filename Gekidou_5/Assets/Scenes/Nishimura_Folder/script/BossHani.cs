using UnityEngine;

public class BossHani : MonoBehaviour
{
    [SerializeField] private int attackPower = 10;     // ボスの攻撃力
    [SerializeField] private float attackInterval = 1.0f; // 次の攻撃までの間隔（秒）

    private float localAttackTimer = 0f; // ボス専用の攻撃タイマー

    void Start()
    {
        // 最初は触れた瞬間にすぐ攻撃できるように、タイマーを満タンにしておく
        localAttackTimer = attackInterval;
    }

    void Update()
    {
        // 毎フレーム、安全にタイマーを進める（通常の敵と同じ仕組み）
        localAttackTimer += Time.deltaTime;
    }

    // プレイヤーに当たった瞬間
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // インターバル時間を満たしている場合だけ攻撃
            if (localAttackTimer >= attackInterval)
            {
                Attack(other.gameObject);
            }
        }
    }

    // プレイヤーに押し当たっている間（継続ダメージ）
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 当たっている間も、インターバル以上の時間が経つたびに再攻撃
            if (localAttackTimer >= attackInterval)
            {
                Attack(other.gameObject);
            }
        }
    }

    // プレイヤーが離れたらタイマーを満タンに戻す
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 次に触れた瞬間にモタつかず、すぐダメージを与えられるようにする
            localAttackTimer = attackInterval;
        }
    }

    // 実際のダメージ処理をまとめた関数
    private void Attack(GameObject playerObj)
    {
        levelupplayer player = playerObj.GetComponent<levelupplayer>();
        if (player != null)
        {
            player.damage(attackPower); // プレイヤーの死亡判定付き関数を呼ぶ
            Debug.Log($"ボスがプレイヤーに {attackPower} ダメージ与えた（残りタイマーリセット）");

            // ダメージを与えたので、タイマーを0にリセットしてカウントし直す
            localAttackTimer = 0f;
        }
    }
}