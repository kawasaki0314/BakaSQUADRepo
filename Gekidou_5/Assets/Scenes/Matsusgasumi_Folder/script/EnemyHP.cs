using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] int maxHp = 5;
    public int currentHp = 5;
    private bool isDead = false;
    private SpriteRenderer sr;

    [Header("被ダメージ点滅設定")]
    [SerializeField] private float blinkDuration = 0.2f; //点滅を続ける時間(秒)
    [SerializeField] private float blinkspeedspeed = 20f;  //点滅の速さ

    private AIHoming aiHoming;

    void Start()
    {
        currentHp = maxHp;
        aiHoming = GetComponent<AIHoming>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        // 点滅
        sr.color = Color.Lerp(Color.white,
                    Color.red,
                    Mathf.PingPong(
                        Time.time * 3f,
                        1f));

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
               // Destroy(gameObject);
            }
        }
    }

    // Triggerで来る弾に対応（BulletPrefabなど）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Trigger]触れたオブジェクト名: {collision.gameObject.name}");

        if (collision.gameObject.name.Contains("AttackBullet") ||
            collision.gameObject.name.Contains("attack1"))
        {
            TakeDamage(1);
           // Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("AttackBullet"))
        {
            TakeDamage(1);
        }
    }
}