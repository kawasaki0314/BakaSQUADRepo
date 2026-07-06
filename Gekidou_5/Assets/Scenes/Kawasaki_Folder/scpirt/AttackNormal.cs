using System.Collections;
using UnityEngine;

public class AttackNormal : MonoBehaviour
{
    public float lifeTime = 0.3f; // 攻撃の存在時間

    [HideInInspector]
    public int attackPower;
    void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ぶつかった相手がプレイヤー自身、処理を無視する
        if(other.CompareTag("Player"))
        {
            return;
        }

        // Enemyタグに当たった場合
        if (other.CompareTag("Enemy"))
        {
            // 当たった相手からEnemyスクリプトを取得する
            Enemy enemy = other.GetComponent<Enemy>();
            
            // 渡された攻撃力分のダメージを与える
            if(enemy != null)
            {
                enemy.TakeDamage(attackPower);
            }
        }
        if (other.CompareTag("Enemy"))
        {
            
            BossHp bossHp = other.GetComponent<BossHp>();
           
            if(bossHp != null)
            {
                bossHp.TakeDamage(attackPower);
            }
        }
    }
}
