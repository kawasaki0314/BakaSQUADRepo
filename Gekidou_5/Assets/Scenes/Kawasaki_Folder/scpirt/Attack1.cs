using System.Collections;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    public float lifeTime = 0.1f; // 攻撃が消えるまでの時間
    [SerializeField]float knockbackPower = 5f; // ノックバックの強さ
    private PlayerMove player; // プレイヤー参照を保持

    public void SetPlayer(PlayerMove p)
    {  
        player = p;
    }
    void Start()
    {
        Destroy(gameObject, lifeTime); // 君消す
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy")) // Enemytagのものに当たったら
        {
            Debug.Log("ヒット!"); // 攻撃が当たっている

            Vector2 knockbackDir = Vector2.right; // 初期値

            if (player != null)
            {
                knockbackDir = player.GetLastDir().normalized;
            }

            // 敵にノックバックを伝える
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Knockback(knockbackDir, knockbackPower);
            }
        }
    }   
    
}