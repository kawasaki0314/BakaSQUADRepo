using UnityEngine;

public class AttackBullet : MonoBehaviour
{
    public float speed = 15f; // 弾の速度
    public float lifeTime = 3f; // 弾の表示時間

    // 攻撃力を受け取るため
    [HideInInspector]
    public int attackPower;

    private Vector3 moveDirection;
    
    // 弾の進む方向を設定する関数
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 表示時間経過後君消す
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // 設定された方向にまっすぐ進む
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // ぶつかった相手がプレイヤー自身、処理を無視する
        if (other.CompareTag("Player"))
        {
            return;
        }

        // Enemyタグに当たった場合
        if (other.CompareTag("Enemy"))
        {
            // 当たった相手からEnemyスクリプトを取得する
            Enemy enemy = other.GetComponent<Enemy>();

            // 渡された攻撃力分のダメージを与える
            if (enemy != null)
            {
                enemy.TakeDamage(attackPower);
            }

            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {

            BossHp bossHp = other.GetComponent<BossHp>();

            if (bossHp != null)
            {
                bossHp.TakeDamage(attackPower);
            }
        }
    }
}
