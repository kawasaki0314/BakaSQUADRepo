
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    public float lifeTime = 0.1f; // 攻撃の存在時間

    void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Enemyタグに当たった場合
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("ヒット!");
        }
    }
}
