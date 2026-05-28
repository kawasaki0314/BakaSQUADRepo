using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player HP settings")]
    [SerializeField] int maxHP = 100; // プレイヤーの最大HP
    int currentHP;

    [Header("Player Attack settings")]
    [SerializeField] int attackPower = 5;      // プレイヤーの攻撃力
    [SerializeField] float attackRadius = 2.0f; // 攻撃が届く範囲（自動なので少し広めの3.0がオススメ）
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
        // 1. 範囲内の敵をすべて取得
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, EnemyLayer);

        // デバッグ用：何匹検知したかコンソールに出す
        if (hitEnemies.Length > 0) Debug.Log(hitEnemies.Length + "匹の敵を検知！");

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // 2. AIHomingスクリプトを取得
            AIHoming enemyScript = enemyCollider.GetComponent<AIHoming>();

            if (enemyScript != null)
            {
                // 3. ダメージ関数を呼ぶ
                enemyScript.TakeDamage(attackPower);
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