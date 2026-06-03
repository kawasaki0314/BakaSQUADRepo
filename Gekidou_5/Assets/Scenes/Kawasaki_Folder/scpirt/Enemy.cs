using UnityEngine;

public class Enemy : MonoBehaviour
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

        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        if (playerLevel != null)
        {
            playerLevel.GainExp(3); // 敵を1体倒したら経験値を「3」手に入れる
        }

        Destroy(gameObject); // エネミーのオブジェクトを削除
    }
}
