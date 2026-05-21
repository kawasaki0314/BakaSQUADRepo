using UnityEngine;

public class AttackOrbit : MonoBehaviour
{
    public Transform player; // 中心のプレイヤー

    public float distance = 1.5f; // 回復半径
    public float speed = 180f; // 回転速度

    private float angle = 0f; // 現在の角度
    
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
}
