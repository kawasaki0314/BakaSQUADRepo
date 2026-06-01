using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject attackPrefab; // 近接攻撃
    public GameObject orbitPrefab; // 回転攻撃
    public GameObject bulletPrefab; // 射撃攻撃

    [Header("Player Stats (Level Up)")]
    public int normalAttackPower = 1; // 近接攻撃の威力
    public int orbitAttackPower = 1; // 回転攻撃の威力
    public int bulletAttackPower = 1; // 射撃攻撃の威力

    [Header("Attack Settings")]
    public float attackOffset = 1.5f; // 近接攻撃を前に出す距離

    [Header("Auto Shoot Settings")]
    public float shootInterval = 2f; // 自動で射撃攻撃を出す間隔
    private float shootTimer = 0f; // 射撃攻撃のタイマー

    private PlayerMove playerMove;

    private float playTime = 0f; // 時間管理

    // 回転攻撃を一度だけ生成するためのフラグ
    private bool orbitSpawned = false;

    void Start()
    {   // PlayerMoveを取得
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        // 時間の更新
        playTime += Time.deltaTime;

        // 一定時間で回転攻撃を出す(1回のみ)
        if (playTime >= 5f && !orbitSpawned)
        {
            orbitSpawned = true;
            SpawnOrbitAttack();
        }

        // 自動で十字型に射撃攻撃を出す
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f; // タイマーリセット
            AutoShoot();
        }

        // 左クリックで近接攻撃
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    void Attack() // 近接攻撃の処理
    {

        if (attackPrefab == null) return;

        // プレイヤーの向いている方向を取得
        Vector2 dir = playerMove.GetLastDir();

        // 攻撃の位置決定
        Vector3 spawnPos = transform.position + (Vector3)(dir * attackOffset);

        // 見た目の調整
        spawnPos.y += 0.2f;

        // 攻撃の向きを回転で合わせる
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 攻撃の生成
        GameObject attackObj = Instantiate(attackPrefab, spawnPos, rot);

        AttackNormal normalScript = attackObj.GetComponent<AttackNormal>();
        if (normalScript != null)
        {
            //normalScript.attackPower = this.normalAttackPower;
        }
    }

    void SpawnOrbitAttack() // 回転攻撃
    {
        if (orbitPrefab == null) return;

        // 0度の位置に攻撃オブジェクトを生成
        CreatOrbit(0f);

        CreatOrbit(180f);
    }

    void CreatOrbit(float angle)
    {
        GameObject orbit = Instantiate(orbitPrefab, transform.position,
           Quaternion.identity);

        AttackOrbit atk = orbit.GetComponent<AttackOrbit>();

        if (atk != null)
        {
            atk.player = transform;// playerを中心にする
            atk.SetStartingAngle(angle); // 180度をセット

            //atk.attackPower = this.orbitAttackPower;
        }
    }

    void AutoShoot() // 十字型に射撃攻撃を出す
    {
        if (bulletPrefab == null) return;

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
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 弾のスクリプトを取得して方向を設定
            AttackBullet bullet = bulletObj.GetComponent<AttackBullet>();
            if (bullet != null)
            {
                bullet.SetDirection(fireDir);

                //bullet.attackPower = this.bulletAttackPower;
            }
        }
    }
}