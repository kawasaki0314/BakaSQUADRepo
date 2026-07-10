using UnityEngine;

public class EnemyHp2 : MonoBehaviour
{

    [Header("HP Settings")]
    [SerializeField] int maxHp = 5;
    public int currentHp = 5;
    private SpriteRenderer sr;

    private AIHoming3 aiHoming3;

    void Start()
    {
        currentHp = maxHp;
        aiHoming3 = GetComponent<AIHoming3>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        // 点滅
        sr.color = Color.Lerp(Color.white,
                    Color.red,
                    Mathf.PingPong(
                        Time.time * 8f,
                        1f));

        Debug.Log($"{gameObject.name}に{damage}のダメージ！残りHP:{currentHp}");

        if (currentHp <= 0)
        {
            if (aiHoming3 != null)
            {
                aiHoming3.Die();
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

        //EnemyBulletタグがついているものは無視
        if (collision.CompareTag("EnemyBullet")) return;

        if (collision.gameObject.name.Contains("AttackBullet") ||
            collision.gameObject.name.Contains("attack1"))
        {
            TakeDamage(1);
           // Destroy(collision.gameObject);
        }
        else if (collision.gameObject.name.Contains("AttackBullet"))
        {
            TakeDamage(1);
        }
    }

}