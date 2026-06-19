using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private AIHoming aiHoming;
    private float localAttackTimer = 0f; // ★この攻撃判定スクリプト専用のタイマー

    void Start()
    {
        aiHoming = GetComponentInParent<AIHoming>();
        // 最初はいつでも攻撃できるように、インターバル以上の値を最初に入れておく
        if (aiHoming != null)
        {
            localAttackTimer = aiHoming.attackInterval;
        }
    }

    void Update()
    {
        // ★毎フレーム、自前で安全にタイマーを進める
        localAttackTimer += Time.deltaTime;
    }

    // プレイヤーに当たった瞬間
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // すでにインターバル時間を満たしている場合だけ攻撃する（重複防止）
            if (localAttackTimer >= aiHoming.attackInterval)
            {
                Attack(collision);
            }
        }
    }

    // プレイヤーに当たり続けている間
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // インターバル以上の時間が経っていたら継続ダメージ
            if (localAttackTimer >= aiHoming.attackInterval)
            {
                Attack(collision);
            }
        }
    }

    // 実際のダメージ処理を一つにまとめる
    private void Attack(Collider2D collision)
    {
        levelupplayer playerHealth = collision.GetComponent<levelupplayer>();
        if (playerHealth != null)
        {
            playerHealth.damage(aiHoming.attackPower);
            Debug.Log($"プレイヤーに {aiHoming.attackPower} ダメージ与えました！");

            // ★ダメージを与えたら、タイマーを確実にリセット
            localAttackTimer = 0f;
        }
    }

    // プレイヤーが離れたらタイマーを最大にして、次に触れた瞬間すぐ攻撃できるようにする
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (aiHoming != null)
            {
                localAttackTimer = aiHoming.attackInterval;
            }
        }
    }
}