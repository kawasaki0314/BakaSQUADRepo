using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;

    [Header("Prefabs")]
    public GameObject attackPrefab; // 近接攻撃
    public GameObject orbitPrefab; // 回転攻撃
    public GameObject bulletPrefab; // 射撃攻撃

    [Header("Player Stats (Level Up)")]
    public int normalAttackPower = 1; // 近接攻撃の威力
    public int orbitAttackPower = 1; // 回転攻撃の威力
    public int bulletAttackPower = 1; // 射撃攻撃の威力

    [Header("Attack Settings")]
    public float attackOffset = 0.5f; // 近接攻撃を前に出す距離
    [SerializeField] LayerMask EnemyLayer;

    public float attackInterval = 0.15f;
    private float attackTimer = 0f;

    [Header("Auto Shoot Settings")]
    public float shootInterval = 2.0f; // 自動で射撃攻撃を出す間隔
    private float shootTimer = 0f; // 射撃攻撃のタイマー

    [Header("Upgrade Stats")]
    public int bulletCount = 4;
    public float fireRateModifier = 0f;

    private PlayerMove playerMove;

    private float playTime = 0f; // 時間管理

    // 回転攻撃を一度だけ生成するためのフラグ
    private bool orbitSpawned = false;

    void Start()
    {   // PlayerMoveを取得
        playerMove = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
    }

    void Update()

    {
        //ゲーム開始前は何もできないゾ
        if(GameStartDirector.IsGameStarted == false) return;

        // 時間の更新
        playTime += Time.deltaTime;

        // ポーズ中なら、これ以降の入力や処理をすべて無視する
        if (PauseManager.IsGamePaused)
        {
            return;
        }

        // 一定時間で回転攻撃を出す(1回のみ)
        if (playTime >= 5f && !orbitSpawned)
        {
            orbitSpawned = true;
            SpawnOrbitAttack();
        }

        // 自動で十字型に射撃攻撃を出す
        shootTimer += Time.deltaTime;

        float currentInterval = Mathf.Max(0.15f, shootInterval - fireRateModifier);
        if (shootTimer >= currentInterval)
        {
            shootTimer = 0f; // タイマーリセット
            AutoShoot();
        }

        // 左クリックで近接攻撃
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }

        attackTimer += Time.deltaTime;

        // 連射
        if (Mouse.current.leftButton.isPressed&&attackTimer>=attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    void Attack() // 近接攻撃の処理
    {

        if (attackPrefab == null) return;

        // プレイヤーの向いている方向(マウスカーソル)を取得
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //Z軸はプレイヤーと同じにして、純粋な２D平面上のベクトルにする
        mouseWorldPos.z = transform.position.z;

        //マウス位置からプレイヤー位置を引いて、方向ベクトルを計算し、正規化する
        Vector2 dir = ((Vector2)(mouseWorldPos - transform.position)).normalized;

        // 攻撃の位置決定
        Vector3 spawnPos = transform.position + (Vector3)(dir * attackOffset);

        // 見た目の調整
        spawnPos.y += 0.2f;

        // 攻撃の向きを回転で合わせる
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 攻撃の生成
        GameObject attackObj = Instantiate(attackPrefab, spawnPos, rot);

        // 攻撃のエフェクト
        //animator.SetTrigger("Normal");

        AttackNormal normalScript = attackObj.GetComponent<AttackNormal>();
        if (normalScript != null)
        {
            normalScript.attackPower = this.normalAttackPower;
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

            atk.attackPower = this.orbitAttackPower;
        }
    }
        // 360度均等に弾を発射する正しい処理
        void AutoShoot()
        {
            if (bulletPrefab == null) return;

        int count = Mathf.Max(1, bulletCount);

        for(int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            GameObject bulletObj = Instantiate(bulletPrefab,
                transform.position, Quaternion.identity);

            // 見た目を進行方向に回転させる
            bulletObj.transform.rotation = Quaternion.Euler(0, 0, angle);

            AttackBullet bullet = bulletObj.GetComponent<AttackBullet>();
            
            if(bullet != null)
            {
                bullet.SetDirection(dir);
                bullet.attackPower = this.bulletAttackPower;
            }
        }
        
        }
public void StartPowerUpBuff(int amount, float duration)
    {
        StartCoroutine(PowerUpRoutine(amount, duration));
    }

    // 攻撃力アップのアイテム
    private System.Collections.IEnumerator PowerUpRoutine(int amount, float duration)
    {
        Debug.Log($"バフ発動、攻撃力が{amount}アップした");

        // 現在の攻撃力＋上昇値
        normalAttackPower += amount;
        orbitAttackPower += amount;
        bulletAttackPower += amount;

        // 指定された時間、ここで実行を一時停止
        yield return new WaitForSeconds(duration);

        // 現在の攻撃力－上昇値
        normalAttackPower -= amount;
        orbitAttackPower -= amount;
        bulletAttackPower -= amount;

        Debug.Log("バフ効果が切れた");
    }
}