using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    Animator animator;

    public float playerSpeed = 5f; // 移動速度

    public float blinkDistance = 7.5f; // ブリンクの距離
    public float blinkDuration = 0.3f; // 時間

    private Vector2 lastDir = Vector2.right; // 最後に入力した方向
    private Rigidbody2D rb; //保持する変数

    // 壁のレイヤーを指定するための変数
    [SerializeField] private LayerMask wallLayerMask;

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
        animator = GetComponent<Animator>();
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

        Anim();

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
            transform.localScale = new Vector3(-3.5f, 3.5f, 1);
            animator.SetBool("run", true);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            moveInput.x += 1;
            transform.localScale = new Vector3(3.5f, 3.5f, 1);
            animator.SetBool("run", true);
        }

        // 上下移動の入力
        if (Keyboard.current.wKey.isPressed)
        {
            moveInput.y += 1;
            animator.SetBool("run", true);
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            moveInput.y -= 1;
            animator.SetBool("run", true);
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
            animator.SetBool("blink", true);
        }
    }

    private void Anim()
    {
        if(rb.linearVelocity.x>0)
        {
            animator.SetBool("run", true);
        }
        if(rb.linearVelocity.y>0)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
       //     animator.SetTrigger("blink");
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

        
        // 元のレイヤーを変更し、ブリンク用のレイヤーに変更
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerBlinking");
        /*
        // プレイヤーのコライダを取得し、一時的にすり抜け状態にする
        Collider2D playerCollider = GetComponent<Collider2D>();
        if(playerCollider != null)
        {
            playerCollider.isTrigger = true;
        }
        */
        // 壁のチェック

        Vector3 start = transform.position;
        Vector3 end;

        // レイキャストでブリンク方向に壁があるかどうかのチェック
        RaycastHit2D hit = Physics2D.Raycast(start, dir, blinkDistance, wallLayerMask);

        if(hit.collider != null)
        {
            // 壁が見つかった場合、ブリンクの調整を行う
            float safeDistance = hit.distance - 0.2f;
            end = start + (Vector3)(dir * safeDistance);
        }
        else
        {
            // 壁がなかったらそのまんまで
            end = start + (Vector3)(dir * blinkDistance);
        }

        float time = 0f;

        while (time < blinkDuration)
        {
            rb.MovePosition(Vector3.Lerp(start, end, time / blinkDuration));
            time += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);
        rb.linearVelocity = Vector2.zero; // 慣性を消す
        /*
        // ブリンク終了後、すり抜け状態を解除し通常状態に戻す
        if(playerCollider != null)
        {
            playerCollider.isTrigger = false;
        }
        */
        gameObject.layer = originalLayer; // 元のレイヤーに戻る
        
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