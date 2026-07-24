using UnityEngine;

public class EnemyHp2 : MonoBehaviour
{

    [Header("HP Settings")]
    [SerializeField] int maxHp = 9;
    public int currentHp = 9;
    private SpriteRenderer sr;
    [Header("被ダメージ点滅設定")]
    [SerializeField] private float blinkDuration = 0.2f; //点滅を続ける時間(秒)
    [SerializeField] private float blinkspeedspeed = 20f;  //点滅の速さ
    [SerializeField, Range(0f, 1f)] private float blinkIntensity = 0.3f; //色の濃さ
    private Coroutine blinkCoroutine;
    private Color originalCoror;
    private bool isDead = false;

    private AIHoming3 aiHoming3;

    void Start()
    {
        currentHp = maxHp;
        aiHoming3 = GetComponent<AIHoming3>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalCoror = sr.color;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHp -= damage;
        // 被ダメージ時の点滅開始
        if (sr != null)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }


        Debug.Log($"{gameObject.name}に{damage}のダメージ！残りHP:{currentHp}");

        if (currentHp <= 0)
        {
            isDead = true; //死亡処理をしたことを記録
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
    private System.Collections.IEnumerator BlinkCoroutine()
    {
        float timer = 0f;

        while (timer < blinkDuration)
        {
            float t = Mathf.PingPong(timer * blinkspeedspeed, 1f) * blinkIntensity;
            sr.color = Color.Lerp(originalCoror, Color.red, t);

            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = originalCoror; // 点滅終了後は元の色に戻す
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