using UnityEngine;

public class AttackOrbit : MonoBehaviour
{
    public Transform player; // 中心のプレイヤー

    public float distance = 2.5f; // 回転半径
    public float speed = 180f; // 回転速度

    private float angle = 0f; // 現在の角度

    public void SetStartingAngle(float startAngle) // 開始角度をセットするための関数
    {
        angle = startAngle;
    }

    void Update()
    {
        if (player == null) return;

        // 角度を増加させて回転させる
        angle += speed * Time.deltaTime;

        // 度→ラジアン変換
        float rad = angle * Mathf.Deg2Rad;

        // 円運動(cos,sin)
        float x = Mathf.Cos(rad) * distance;
        float y = Mathf.Sin(rad) * distance;

        // プレイヤー中心に配置
        transform.position = player.position + new Vector3(x, y, 0);
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
