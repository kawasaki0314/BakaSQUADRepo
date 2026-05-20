
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackPrefab; // 攻撃プレハブ
    public float attackOffset = 1.0f; // 前に出す距離

    private PlayerMove playerMove;

    void Start()
    {   // PlayerMoveを取得
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {   // 左クリックで攻撃
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    void Attack()
    {   // プレイヤーの向いている方向を取得
        Vector2 dir = playerMove.GetLastDir();

        // 攻撃の位置決定
        Vector3 spawnPos = transform.position + (Vector3)(dir * attackOffset);

        // 見た目の調整
        spawnPos.y += 0.2f;

        // 攻撃の向きを回転で合わせる
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 攻撃の生成
        Instantiate(attackPrefab, spawnPos, rot);
    }
}
