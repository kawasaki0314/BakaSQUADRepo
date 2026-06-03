using UnityEngine;

public class AttackBullet : MonoBehaviour
{
    public float speed = 8f; // 弾の速度
    public float lifeTime = 3f; // 弾の表示時間

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
        // Enemyタグに当たった場合
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("ヒット!");
            Destroy(gameObject);
        }
    }
}
