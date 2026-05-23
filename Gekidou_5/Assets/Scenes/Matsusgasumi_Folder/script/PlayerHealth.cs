using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player HP settings")]
    [SerializeField] int maxHP = 100; // プレイヤーの最大HP
    int currentHP;

    [Header("Player Attack settings")]
    [SerializeField] int attackPower = 5;      // プレイヤーの攻撃力
    [SerializeField] float attackRadius = 3.0f; // 攻撃が届く範囲（自動なので少し広めの3.0がオススメ）
    [SerializeField] LayerMask EnemyLayer;      // 敵のレイヤー（Enemy）
    [SerializeField] float attackInterval = 1.0f; // 攻撃の間隔（何秒に1回攻撃するか）

    float attackTimer = 0f; // タイマー用の変数

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        // ==========================================
        // ★【自動化】時間をカウントして、一定時間ごとに自動で攻撃
        // ==========================================
        attackTimer += Time.deltaTime; // 毎フレーム、時間を足していく

        if (attackTimer >= attackInterval)
        {
            Attack();        // 攻撃を実行
            attackTimer = 0f; // タイマーをリセットして、また0秒からカウント
        }
    }

    // 自動攻撃の処理
    void Attack()
    {
        // 自分の周りの範囲内にいる敵のコライダーをすべて検知する
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, EnemyLayer);

        // 範囲内に敵が1匹もいなければ、ログも出さずに処理を終える（スッキリさせるため）
        if (hitEnemies.Length == 0) return;

        Debug.Log("自動攻撃が発動！");

        // 検知した敵すべてにダメージを与える
        foreach (Collider2D Enemy in hitEnemies)
        {
            AIHoming EnemyScript = Enemy.GetComponent<AIHoming>();

            if (EnemyScript != null)
            {
                EnemyScript.TakeDamage(attackPower);
                Debug.Log($"{Enemy.name} に自動で {attackPower} のダメージを与えた！");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"プレイヤーが{damage}のダメージを受けた! 残りHP:{currentHP}");

        if (currentHP <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("プレイヤーは倒された...");
        gameObject.SetActive(false);
    }

    public int GetcurrentHP()
    {
        return currentHP;
    }

    // シーン画面で攻撃範囲を視覚的に見えるようにする
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}