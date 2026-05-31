using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [Header("Enemy HP Settings")]
    public int maxHp = 3;  // エネミーの最大HP
    private int nowHp;     // 現在のHP

    void Start()
    {
        // ゲーム開始時にHPを満タンにする
        nowHp = maxHp;
    }

    // ダメージを受ける関数（プレイヤーの攻撃から呼び出される）
    public void TakeDamage(int damage)
    {
        nowHp -= damage;
        Debug.Log($"【エネミー】ダメージを {damage} 受けた！ 残りHP: {nowHp}");

        // HPが0以下になったら消滅
        if (nowHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("エネミーを倒した！");
        Destroy(gameObject); // エネミーのオブジェクトを削除
    }
}
