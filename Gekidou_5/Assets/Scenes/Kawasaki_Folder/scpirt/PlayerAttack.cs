using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackPrefab; // 攻撃プレハブ
    public GameObject orbitPrefab; // 攻撃プレハブ２
    public float attackOffset = 1.0f; // 前に出す距離

    private PlayerMove playerMove;

    private float playTime = 0f; // 時間管理

    // 一度だけ生成するためのフラグ
    private bool orbitSpawned = false;

    void Start()
    {   // PlayerMoveを取得
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        // 経過時間を増やす
        playTime += Time.deltaTime;

        // 一定時間で回転攻撃を出す(1回のみ)
        if (playTime >= 30f && !orbitSpawned)
        {
            orbitSpawned = true;
            SpawnOrbitAttack();
        }

        // 左クリックで攻撃
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

    void SpawnOrbitAttack()
    {
        GameObject orbit = Instantiate(orbitPrefab, transform.position,
            Quaternion.identity);

        AttackOrbit atk = orbit.GetComponent<AttackOrbit>();

        // プレイヤーを中心にする
        atk.player = transform;
    }
}
