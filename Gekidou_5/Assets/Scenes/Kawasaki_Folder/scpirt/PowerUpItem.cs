using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [Header("Buff Setteingd")]
    public int powerValue = 1; // 攻撃力の上昇値
    public float duration = 10f; // 効果持続時間

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
            if(playerAttack != null)
            {
                playerAttack.StartPowerUpBuff(powerValue, duration);
                Destroy(gameObject);
            }
            
       
        }
    }
}
