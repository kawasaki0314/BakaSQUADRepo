using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] int maxHp = 65;
    public int currentHp = 65;
    private bool isDead = false;

    private AIHoming aiHoming;

    void Start()
    {
        currentHp = maxHp;
        aiHoming = GetComponent<AIHoming>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        Debug.Log($"{gameObject.name}に{damage}のダメージ！残りHP:{currentHp}");

        if (currentHp <= 0)
        {
            isDead = true;
            if (aiHoming != null)
            {
                aiHoming.Die();
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