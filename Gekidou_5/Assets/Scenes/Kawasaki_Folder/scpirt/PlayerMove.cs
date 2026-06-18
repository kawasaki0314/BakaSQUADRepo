using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5f; // 移動速度

    public float blinkDistance = 7.5f; // ブリンクの距離
    public float blinkDuration = 0.3f; // 時間

    private Vector2 lastDir = Vector2.right; // 最後に入力した方向
    private Rigidbody2D rb; //保持する変数

    public Vector2 GetLastDir()
    {
        return lastDir;
    }
    public float blinkCooldown = 3.0f; // クールダウンタイム
    private float cooldownTimer = 0f;
    public float GetCoolDownTimer()
    {
        return cooldownTimer;
    }

    private bool isBlinking = false;
    private Vector2 moveInput = Vector2.zero; // 【追加】入力を保持する変数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ポーズ中なら、これ以降の入力や処理をすべて無視する
        if (PauseManager.IsGamePaused)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            moveInput = Vector2.zero; // 入力もリセット
            return;
        }

        // キーボードの認識チェック
        if (Keyboard.current == null) return;

        // クールダウン
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 入力をリセット
        moveInput = Vector2.zero;

        // 左右移動の入力
        if (Keyboard.current.aKey.isPressed)
        {
            moveInput.x -= 1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            moveInput.x += 1;
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 上下移動の入力
        if (Keyboard.current.wKey.isPressed)
        {
            moveInput.y += 1;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            moveInput.y -= 1;
        }

        // 方向の更新(ブリンクなどに使用)
        if (moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized;
            lastDir = moveInput;
        }

        // 右クリックでブリンク
        if (Mouse.current.rightButton.wasPressedThisFrame &&
            lastDir != Vector2.zero &&
            !isBlinking &&
            cooldownTimer <= 0f)
        {
            StartCoroutine(Blink(lastDir));
        }
    }

    // 【追加】物理演算の更新タイミングで速度を強制固定する
    void FixedUpdate()
    {
        // ブリンク中、またはポーズ中はFixedUpdateでの速度上書きをしない
        if (isBlinking || PauseManager.IsGamePaused) return;

        // 敵にぶつかろうが何だろうが、キー入力に応じた速度で完全に上書きする
        rb.linearVelocity = moveInput * playerSpeed;
    }

    // ブリンクの処理
    IEnumerator Blink(Vector2 dir)
    {
        isBlinking = true;
        cooldownTimer = blinkCooldown;

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(dir * blinkDistance);

        float time = 0f;

        while (time < blinkDuration)
        {
            rb.MovePosition(Vector3.Lerp(start, end, time / blinkDuration));
            time += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);

        rb.linearVelocity = Vector2.zero; // 慣性を消す
        isBlinking = false;               // 通常移動を再開
    }

    // スピードアップのバフアイテム
    public void StartSpeedUpBuff(int amount, float duration)
    {
        StartCoroutine(SpeedUpRoutine(amount, duration));
    }

    private System.Collections.IEnumerator SpeedUpRoutine(int amount, float duration)
    {
        Debug.Log($"バフ発動、移動速度が{amount}アップした");
        playerSpeed += amount;
        yield return new WaitForSeconds(duration);
        playerSpeed -= amount;
        Debug.Log("バフ効果が切れた");
    }
}