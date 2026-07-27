using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int damage = 1; //弾の攻撃力
    [SerializeField] float lifeTime = 3.5f;  //消滅するまでの時間（秒）
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //５秒経ったら画面外にいなくても自動で消えるようにする（メモリ対策）
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーに当たったら
        if (collision.CompareTag("Player"))
        {
            levelupplayer playerHealth = collision.GetComponent<levelupplayer>();
            if (playerHealth != null)
            {
                playerHealth.damage(damage); //ダメージを与える（関数名も damage() に統一）
            }
            Destroy(gameObject); //弾を消す
        }
    }
 }
