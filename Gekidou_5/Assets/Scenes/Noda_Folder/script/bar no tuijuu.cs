using UnityEngine;

public class barnotuijuu : MonoBehaviour
{
    [SerializeField] private Transform target; // プレイヤーのTransformを指定
    [SerializeField] private Vector3 offset;   // プレイヤー頭上へのオフセット（例: 0, 1.5, 0）

    void LateUpdate()
    {
        if (target != null)
        {
            // 位置だけを同期させ、回転やスケールは無視する
            transform.position = target.position + offset;
        }
    }
}