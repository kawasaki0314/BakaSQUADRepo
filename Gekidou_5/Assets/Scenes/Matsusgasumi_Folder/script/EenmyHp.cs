using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    
    [Header("HP Settings")]
    [SerializeField] int maxHp = 8;
    public int currentHp = 8;
    private bool isDead = false;
    private AIHoming2 aiHoming2;

    void Start()
    {
        currentHp = maxHp;
        aiHoming2 = GetComponent<AIHoming2>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHp -= damage;
        Debug.Log($"{gameObject.name}に{damage}のダメージ！残りHP:{currentHp}");

        if (currentHp <= 0)
        {
            isDead = true;
            if (aiHoming2 != null)
            {
               aiHoming2.Die();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Triggerで来る弾に対応（BulletPrefabなど）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Trigger]触れたオブジェクト名: {collision.gameObject.name}");

        if (collision.gameObject.name.Contains("Bullet") ||
            collision.gameObject.name.Contains("attack1"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.name.Contains("Orbit"))
        {
            TakeDamage(1);
        }
    }
    
}