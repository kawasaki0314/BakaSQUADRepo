using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject attackPrefab; // 攻撃プレハブ
    public GameObject orbitPrefab; // 攻撃プレハブ2
    public GameObject BulletPrefab; // 攻撃プレハブ3

    [Header("Attack Settings")]

    public float attackOffset = 1.5f; // 攻撃1を前に出す距離

    [Header("Auto Shoot Settings")]
    public float shootInterval = 2f; // 自動で攻撃3を出す間隔
    private float shootTimer = 0f; // 自動攻撃のタイマー

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
        if (playTime >= 5f && !orbitSpawned)
        {
            orbitSpawned = true;
            SpawnOrbitAttack();
        }

        // 自動で十字型に攻撃3を出す
        shootTimer += Time.deltaTime;
        if(shootTimer >= shootInterval)
        {
            shootTimer = 0f; // タイマーリセット
            AutoShoot();
        }

        // 左クリックで攻撃
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    void Attack() // 通常攻撃の処理
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

    void SpawnOrbitAttack() // 回転攻撃の処理
    {
        // 1個目の攻撃を生成(0度の位置からスタート)
        GameObject orbit1 = Instantiate(orbitPrefab, transform.position,
            Quaternion.identity);
        AttackOrbit atk1 = orbit1.GetComponent<AttackOrbit>();
        if (atk1 != null)
        {
            atk1.player = transform;// playerを中心にする
            atk1.SetStartingAngle(0f); // 0度をセット
        }
        
        // 2個目の攻撃を生成(180度の位置からスタート)
        GameObject orbit2 = Instantiate(orbitPrefab, transform.position,
            Quaternion.identity);
        AttackOrbit atk2 = orbit2.GetComponent<AttackOrbit>();
        if (atk2 != null)
        {
            atk2.player = transform;// playerを中心にする
            atk2.SetStartingAngle(180f); // 180度をセット
        }
    }

    void AutoShoot()
    {
        if (BulletPrefab == null) return;

        // 十字の4方向のベクトル配置
        Vector2[] ShootDirections =
        {
            Vector2.right, // (1,0)
            Vector2.left, // (-1,0)
            Vector2.up, // (0,1)
            Vector2.down // (0,-1)
        };

        // ループ処理で4回弾を生成し、それぞれ方向に飛ばす
        foreach (Vector2 fireDir in ShootDirections)
        {
            // 弾をプレイヤーの位置に生成
            GameObject bulletObj = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            // 弾のスクリプトを取得して方向を設定
            AttackBullet bullet = bulletObj.GetComponent<AttackBullet>();
            if(bullet != null)
            {
                bullet.SetDirection(fireDir);
            }
        }
    }

}

