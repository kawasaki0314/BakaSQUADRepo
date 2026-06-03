using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 2f; // 移動速度

    public float blinkDistance = 7.5f; // ブリンクの距離
    public float blinkDuration = 0.3f; // 時間

    private Vector2 lastDir = Vector2.right; // 最後に入力した方向

    public Vector2 GetLastDir()　// 他のスクリプトから方向を所得するための関数
    {
        return lastDir;
    }
    public float blinkCooldown = 3.0f; // クールダウンタイム
    private float cooldownTimer = 0f;

    private bool isBlinking = false;
    void Update()
    {
        Vector2 move = Vector2.zero;

        // キーボードの認識チェック
        if (Keyboard.current == null) return;

        // クールダウン
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 左右移動の入力
        if (Keyboard.current.aKey.isPressed)
        {
            move.x -= 1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Dを押すと右移動,右を向く
        else if (Keyboard.current.dKey.isPressed)
        {
            move.x += 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        // 上下移動の入力
        if (Keyboard.current.wKey.isPressed)
        {
            move.y += 1;
        }
        // Sを押すと下移動
        else if (Keyboard.current.sKey.isPressed)
        {
            move.y -= 1;
        }

        // 方向の更新(ブリンクなどに使用)
        if (move != Vector2.zero)
        {
            move = move.normalized;
            lastDir = move;
        }

        // ブリンク中は通常移動しない
        if (!isBlinking)
        {
            transform.position += (Vector3)(move * playerSpeed * Time.deltaTime);
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

    // ブリンクの処理
    IEnumerator Blink(Vector2 dir)
    {
        // ブリンク中フラグon(移動を止めるため)
        isBlinking = true;
        // クールダウン開始
        cooldownTimer = blinkCooldown;

        // 現在位置の記録
        Vector3 start = transform.position;
        // 終了位置の計算
        Vector3 end = start + (Vector3)(dir * blinkDistance);

        float time = 0f;

        // 一定時間かけて移動
        while (time < blinkDuration)
        {
            transform.position = Vector3.Lerp(start, end, time / blinkDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isBlinking = false;
    }
}
