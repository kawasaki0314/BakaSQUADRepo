using UnityEngine;

public class DinasoarSpawn : MonoBehaviour
{
    [Header("移動速度の範囲")]
    public float minSpeed = 4.0f; // 最低速度
    public float maxSpeed = 10.0f; // 最高速度

    private float moveSpeed;

    void Start()
    {
        // 出現した瞬間に、範囲内からランダムな速度を決定する
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // 決定された速度で右に移動
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}