using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [Header("Buff Setteingd")]
    public int powerValue = 2; // 攻撃力の上昇値
    public float duration = 10f; // 効果持続時間

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 触れた相手がプレイヤーだった場合
        if (other.CompareTag("Player"))
        {
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                // PlayerAttack側の受付窓口を呼び出す
                playerAttack.StartPowerUpBuff(powerValue, duration);

                Destroy(gameObject); // 君消す
            }
        }
    }
}
