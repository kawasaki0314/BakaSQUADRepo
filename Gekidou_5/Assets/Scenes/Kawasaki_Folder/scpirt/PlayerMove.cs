using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using JetBrains.Annotations;

public class PlayerMove : MonoBehaviour
{
    public float speed = 2f;
    // ブリンクの距離と時間
    public float blinkDistance = 7.5f;
    public float blinkDuration = 0.3f;

    public Vector2 GetLastDir()
    {
        return lastDir;
    }

    private Vector2 lastDir = Vector2.right; // 方向を記録する変数

    [Header("Blink Cooldown")]
    public float blinkCooldown = 3.0f; // ブリンクのクールダウン時間
    private float cooldownTimer = 0f;  // ブリンクのクールダウンタイマー

    private bool isBlinking = false;
    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;

        // エラーを防ぐため
        if (Keyboard.current == null) return;
        // ブリンク中は移動を無効化
        if (isBlinking) return;

        // クールダウンタイマーのカウントダウン
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Aを押すと左移動,左を向く
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
        // Wを押すと上移動
        if (Keyboard.current.wKey.isPressed)
        {
            move.y += 1;
        }
        // Sを押すと下移動
        else if (Keyboard.current.sKey.isPressed)
        {
            move.y -= 1;
        }

        // 斜め移動でも速度を同じにするやつ
        if (move != Vector2.zero)
        {
            move.Normalize();
            lastDir = move;
        }

        // ブリンク中は移動を止める
        if (!isBlinking)
        {
            transform.position += (Vector3)(move * speed * Time.deltaTime);
        }

        // 右クリックするとブリンク
        // 右クリックを検出&移動入力がある&ブリンク中でない&クールダウンが終わっている
        if (Mouse.current.rightButton.wasPressedThisFrame && lastDir !=
            Vector2.zero && !isBlinking && cooldownTimer <= 0f)
        {
            StartCoroutine(Blink(lastDir));
        }

    }
    IEnumerator Blink(Vector2 dir)
    {
        isBlinking = true;

        // ブリンク開始時にクールダウンタイマーを設定
        cooldownTimer = blinkCooldown;
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(dir * blinkDistance);

        float time = 0f;
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